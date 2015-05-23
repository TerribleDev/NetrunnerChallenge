using System.ComponentModel.DataAnnotations;

namespace NetrunnerChallenge.Models
{
    public class ChallengeCard
    {
        public string Code { get; set; }
        public int? FactionCosts { get; set; }
        public string Faction { get; set; }
        [Key]
        public int Id { get; set; } 
    }
}