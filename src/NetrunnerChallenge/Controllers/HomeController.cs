using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NetrunnerChallenge.Data;
using NetrunnerChallenge.Extensions;
using NetrunnerChallenge.Models;
using NetrunnerDb.Net.Responses;

namespace NetrunnerChallenge.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = int.MaxValue)]
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
      
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
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
               
                    return View("Challenge", viewModel);
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
                var currentChallenges = db.ChallengeDb.ToList();
                while (rnmId == null || currentChallenges.Any(a => string.Equals(a.Id, rnmId)))
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
                    var choice = cards[index];
                    while ((challenge.CurrentInfluence +
                            (string.Equals(choice.Faction, challenge.Faction.FactionName,
                                StringComparison.CurrentCultureIgnoreCase)
                                ? 0
                                : choice.Factioncost)) > 15)
                    {
                        index = randgen.Next(length);
                        choice = cards[index];
                    }
                    
                    
                    var chal = new ChallengeCard {Code = choice.Code, Faction = choice.Faction, FactionCosts = choice.Factioncost};
                    db.Entry(chal).State = EntityState.Added;
                    challenge.Cards.Add(chal);
                }
                db.ChallengeDb.Add(challenge);
                db.SaveChanges();
                return RedirectToAction("Challenge", "Home", new {id = challenge.Id});
            }
        }
    }
}