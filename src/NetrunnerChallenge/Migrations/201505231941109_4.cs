using System.Linq;
using NetrunnerChallenge.Data;

namespace NetrunnerChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChallengeCards", "FactionCosts", c => c.Int());
            AddColumn("dbo.ChallengeCards", "Faction", c => c.String());
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChallengeCards", "Faction");
            DropColumn("dbo.ChallengeCards", "FactionCosts");
        }
    }
}
