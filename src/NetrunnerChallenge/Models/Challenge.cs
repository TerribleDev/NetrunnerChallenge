using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NetrunnerChallenge.Extensions;

namespace NetrunnerChallenge.Models
{
    public class Challenge
    {
        [Key]
        public string Id { get; set; }
        public ICollection<ChallengeCard> Cards { get; set; }
        public Faction Faction { get; set; }
        public DateTime CreationDate { get; set; }
        [NotMapped]
        public int CurrentInfluence {
            get
            {
               return Cards.Where(a => !string.Equals(Faction.FactionName, a.Faction) && a.FactionCosts.HasValue)
                    .Select(a => (int) a.FactionCosts).Combine();
            } }
    }
}