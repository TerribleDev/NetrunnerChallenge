﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetrunnerChallenge.Data;
using NetrunnerChallenge.Extensions;
using NetrunnerChallenge.Models;
using NetrunnerDb.Net.Responses;
using Newtonsoft.Json;

namespace NetrunnerChallenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new DatabaseContext())
            {
                var mods = db.FactionsDb
                    .ToList()
                    .Where(a => !string.Equals(a.FactionName, "Neutral", StringComparison.CurrentCultureIgnoreCase))
                    .Select(a => a.FactionName)
                    .ToList();
                mods.Insert(0, "Random");
                return View(mods);
            }
                
        }
      
        /// <exception cref="NotImplementedException">Always.</exception>
        public ActionResult Challenge(string id)
        {
            using (var db = new DatabaseContext())
            {
                var challege = db.ChallengeDb
                    .Include(a=>a.Faction)
                    .Include(a=>a.Cards)
                    .FirstOrDefault(a => a.Id == id);
                if (challege == null)
                {
                    Response.StatusCode = 404;
                    ViewBag.ErrorCode = 404;
                    return View("Error");
                }
                var cards = new ConcurrentBag<OneCard>();
                challege.Cards.AsParallel().ForAll(a =>
                {
                    cards.Add(new NetrunnerApi().GetCard(a.Code).First());
                });
                var viewModel = new ChallengeResultViewModel {Cards = cards, Challenge = challege};
                if (this.HttpContext.Request.IsAjaxRequest())
                {
                    return PartialView("Challenge",viewModel);
                }
                else
                {
                    return View("Challenge", viewModel);
                }
                
            }
        }

        [HttpPost]
        public ActionResult Generate(string faction, int cardAmount = 5)
        {
            using (var db = new DatabaseContext())
            {
                var dbFaction = db.FactionsDb.ToList();
                var factionMatch = string.Equals(faction, "random", StringComparison.InvariantCultureIgnoreCase)
                    ? dbFaction
                        .Where(a => !string.Equals(a.FactionName, "Neutral", StringComparison.CurrentCultureIgnoreCase))
                        .RandomElement() 
                    : dbFaction
                        .FirstOrDefault(a => string.Equals(a.FactionName, faction, StringComparison.CurrentCultureIgnoreCase));

                if (factionMatch == null)
                {
                    throw new Exception(string.Format("{0} is invalid", faction));
                }
                var generator = new Generator();
                string rnmId = null;
                while (rnmId == null || db.ChallengeDb.ToList().Any(a => string.Equals(a.Id, rnmId)))
                {
                    rnmId = generator.GenerateRandomString();
                }
                var challenge = new Challenge
                {
                    Id = rnmId,
                    CreationDate = DateTime.UtcNow,
                    Faction = factionMatch,
                    Cards = new List<ChallengeCard>()
                };
                var cards = new NetrunnerApi().GetCards()
                    .Where(a => string.Equals(a.Side, factionMatch.Side, StringComparison.CurrentCultureIgnoreCase) && a.Agendapoints == null && !string.IsNullOrWhiteSpace(a.Imagesrc))
                    .ToList();
                var length = cards.Count - 1;
                var randgen = new Random();
                for (int i = 0; i < cardAmount; i++)
                {
                    var index = randgen.Next(length);
                    var chal = new ChallengeCard {Code = cards[index].Code};
                    db.Entry(chal).State = EntityState.Added;
                    challenge.Cards.Add(chal);
                }
                db.ChallengeDb.Add(challenge);
                db.SaveChanges();
                return Challenge(challenge.Id);
            }
        }
    }
}