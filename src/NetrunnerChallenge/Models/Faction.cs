using System.ComponentModel.DataAnnotations;

namespace NetrunnerChallenge.Models
{
    public class Faction
    {
        [Key]
        public int Id { get; set; }
        public string FactionName { get; set; }  
        public string Side { get; set; }
    }
}