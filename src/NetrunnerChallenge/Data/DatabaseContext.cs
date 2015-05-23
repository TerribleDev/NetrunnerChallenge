using NetrunnerChallenge.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NetrunnerChallenge.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        :base("name=DbContext"){ }
        public DbSet<Challenge> ChallengeDb { get; set; }
        public DbSet<Faction> FactionsDb { get; set; }
    }
}