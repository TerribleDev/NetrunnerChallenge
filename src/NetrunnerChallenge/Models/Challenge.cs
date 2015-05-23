using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace NetrunnerChallenge.Models
{
    public class Challenge
    {
        [Key]
        public string Id { get; set; }
        public ICollection<ChallengeCard> Cards { get; set; }
        public Faction Faction { get; set; }
        public DateTime CreationDate { get; set; }
    }
}