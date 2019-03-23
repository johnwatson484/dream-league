namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCupModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Cup.CupFixtures",
                c => new
                    {
                        FixtureId = c.Int(nullable: false, identity: true),
                        CupId = c.Int(nullable: false),
                        GameWeekId = c.Int(nullable: false),
                        HomeManagerId = c.Int(nullable: false),
                        AwayManagerId = c.Int(nullable: false),
                        Round = c.Int(nullable: false),
                        Manager_ManagerId = c.Int(),
                    })
                .PrimaryKey(t => t.FixtureId)
                .ForeignKey("DreamLeague.Managers", t => t.Manager_ManagerId)
                .ForeignKey("DreamLeague.Managers", t => t.AwayManagerId, cascadeDelete: false)
                .ForeignKey("Cup.Cups", t => t.CupId, cascadeDelete: true)
                .ForeignKey("DreamLeague.GameWeeks", t => t.GameWeekId, cascadeDelete: false)
                .ForeignKey("DreamLeague.Managers", t => t.HomeManagerId, cascadeDelete: false)
                .Index(t => t.CupId)
                .Index(t => t.GameWeekId)
                .Index(t => t.HomeManagerId)
                .Index(t => t.AwayManagerId)
                .Index(t => t.Manager_ManagerId);
            
            CreateTable(
                "Cup.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        CupId = c.Int(nullable: false),
                        Name = c.Int(nullable: false),
                        GroupLegs = c.Int(nullable: false),
                        TeamsAdvancing = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("Cup.Cups", t => t.CupId, cascadeDelete: true)
                .Index(t => t.CupId);
            
            CreateTable(
                "Cup.Cups",
                c => new
                    {
                        CupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        HasGroupStage = c.Boolean(nullable: false),
                        KnockoutLegs = c.Int(nullable: false),
                        FinalLegs = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CupId);
            
            CreateTable(
                "dbo.ManagerGroups",
                c => new
                    {
                        ManagerId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ManagerId, t.GroupId })
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .ForeignKey("Cup.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.ManagerId)
                .Index(t => t.GroupId);
            
            AddColumn("Results.Conceded", "Cup", c => c.Boolean(nullable: false));
            AddColumn("Results.Goals", "Cup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Cup.CupFixtures", "HomeManagerId", "DreamLeague.Managers");
            DropForeignKey("Cup.CupFixtures", "GameWeekId", "DreamLeague.GameWeeks");
            DropForeignKey("Cup.Groups", "CupId", "Cup.Cups");
            DropForeignKey("Cup.CupFixtures", "CupId", "Cup.Cups");
            DropForeignKey("Cup.CupFixtures", "AwayManagerId", "DreamLeague.Managers");
            DropForeignKey("dbo.ManagerGroups", "GroupId", "Cup.Groups");
            DropForeignKey("dbo.ManagerGroups", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("Cup.CupFixtures", "Manager_ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.ManagerGroups", new[] { "GroupId" });
            DropIndex("dbo.ManagerGroups", new[] { "ManagerId" });
            DropIndex("Cup.Groups", new[] { "CupId" });
            DropIndex("Cup.CupFixtures", new[] { "Manager_ManagerId" });
            DropIndex("Cup.CupFixtures", new[] { "AwayManagerId" });
            DropIndex("Cup.CupFixtures", new[] { "HomeManagerId" });
            DropIndex("Cup.CupFixtures", new[] { "GameWeekId" });
            DropIndex("Cup.CupFixtures", new[] { "CupId" });
            DropColumn("Results.Goals", "Cup");
            DropColumn("Results.Conceded", "Cup");
            DropTable("dbo.ManagerGroups");
            DropTable("Cup.Cups");
            DropTable("Cup.Groups");
            DropTable("Cup.CupFixtures");
        }
    }
}
