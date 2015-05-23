using System.Data.Entity.Migrations;

namespace NetrunnerChallenge.Migrations
{
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
