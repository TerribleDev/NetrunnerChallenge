namespace NetrunnerChallenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChallengeCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Challenge_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id)
                .Index(t => t.Challenge_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChallengeCards", "Challenge_Id", "dbo.Challenges");
            DropIndex("dbo.ChallengeCards", new[] { "Challenge_Id" });
            DropTable("dbo.ChallengeCards");
        }
    }
}
