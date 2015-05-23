using System.Data.Entity;
using NetrunnerChallenge.Models;

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