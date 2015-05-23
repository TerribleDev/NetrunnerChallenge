using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetrunnerDb.Net.Responses;

namespace NetrunnerChallenge.Models
{
    public class ChallengeResultViewModel
    {
        public IEnumerable<OneCard> Cards { get; set; }
        public Challenge Challenge { get; set; } 
    }
}