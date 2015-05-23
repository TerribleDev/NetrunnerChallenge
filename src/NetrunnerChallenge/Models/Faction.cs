using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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