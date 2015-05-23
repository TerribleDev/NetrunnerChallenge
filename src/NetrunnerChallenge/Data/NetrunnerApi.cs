using System;
using System.Collections.Generic;
using Glav.CacheAdapter.Core.DependencyInjection;
using NetrunnerDb.Net;
using NetrunnerDb.Net.Responses;

namespace NetrunnerChallenge.Data
{
    /// <summary>
    /// Wraps the netrunner API in a cache so we don't keep requesting data when we don't need to
    /// </summary>
    public class NetrunnerApi
    {
        public IList<Cards> GetCards()
        {
            return AppServices.Cache.Get("AllCards",DateTime.Today.AddDays(5), () => new Repository().GetCards());

        }

        public IList<OneCard> GetCard(string code)
        {
            return AppServices.Cache.Get(code + "-Card", DateTime.MaxValue, () => new Repository().GetCard(code));

        }
    }
}