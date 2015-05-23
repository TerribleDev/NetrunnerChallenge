namespace NetrunnerChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Factions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FactionName = c.String(),
                        Side = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Challenges", "Faction_Id", c => c.Int());
            CreateIndex("dbo.Challenges", "Faction_Id");
            AddForeignKey("dbo.Challenges", "Faction_Id", "dbo.Factions", "Id");
            DropColumn("dbo.Challenges", "Faction");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Challenges", "Faction", c => c.Int(nullable: false));
            DropForeignKey("dbo.Challenges", "Faction_Id", "dbo.Factions");
            DropIndex("dbo.Challenges", new[] { "Faction_Id" });
            DropColumn("dbo.Challenges", "Faction_Id");
            DropTable("dbo.Factions");
        }
    }
}
