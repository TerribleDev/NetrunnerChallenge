using System.Collections.Generic;
using NetrunnerDb.Net.Responses;

namespace NetrunnerChallenge.Models
{
    public class ChallengeResultViewModel
    {
        public IEnumerable<OneCard> Cards { get; set; }
        public Challenge Challenge { get; set; } 
    }
}