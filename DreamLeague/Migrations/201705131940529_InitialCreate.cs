namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Results.Conceded",
                c => new
                    {
                        ConcedeId = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        GameWeekId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConcedeId)
                .ForeignKey("DreamLeague.GameWeeks", t => t.GameWeekId, cascadeDelete: true)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .ForeignKey("League.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.GameWeekId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "DreamLeague.GameWeeks",
                c => new
                    {
                        GameWeekId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        Complete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GameWeekId);
            
            CreateTable(
                "DreamLeague.Managers",
                c => new
                    {
                        ManagerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ManagerId);
            
            CreateTable(
                "DreamLeague.Emails",
                c => new
                    {
                        EmailId = c.Int(nullable: false, identity: true),
                        ManagerId = c.Int(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.EmailId)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "DreamLeague.ManagerGoalKeepers",
                c => new
                    {
                        ManagerGoalKeeperId = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        Substitute = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ManagerGoalKeeperId)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .ForeignKey("League.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "League.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        LeagueId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("League.Leagues", t => t.LeagueId, cascadeDelete: true)
                .Index(t => t.LeagueId);
            
            CreateTable(
                "League.Leagues",
                c => new
                    {
                        LeagueId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LeagueId);
            
            CreateTable(
                "League.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("League.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "DreamLeague.ManagerPlayers",
                c => new
                    {
                        ManagerPlayerId = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        Substitute = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ManagerPlayerId)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .ForeignKey("League.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "Results.Goals",
                c => new
                    {
                        GoalId = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        GameWeekId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GoalId)
                .ForeignKey("DreamLeague.GameWeeks", t => t.GameWeekId, cascadeDelete: true)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId, cascadeDelete: true)
                .ForeignKey("League.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.GameWeekId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Manager_ManagerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DreamLeague.Managers", t => t.Manager_ManagerId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Manager_ManagerId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Manager_ManagerId", "DreamLeague.Managers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Results.Goals", "PlayerId", "League.Players");
            DropForeignKey("Results.Goals", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("Results.Goals", "GameWeekId", "DreamLeague.GameWeeks");
            DropForeignKey("Results.Conceded", "TeamId", "League.Teams");
            DropForeignKey("Results.Conceded", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("League.Players", "TeamId", "League.Teams");
            DropForeignKey("DreamLeague.ManagerPlayers", "PlayerId", "League.Players");
            DropForeignKey("DreamLeague.ManagerPlayers", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("DreamLeague.ManagerGoalKeepers", "TeamId", "League.Teams");
            DropForeignKey("League.Teams", "LeagueId", "League.Leagues");
            DropForeignKey("DreamLeague.ManagerGoalKeepers", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("DreamLeague.Emails", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("Results.Conceded", "GameWeekId", "DreamLeague.GameWeeks");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Manager_ManagerId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("Results.Goals", new[] { "ManagerId" });
            DropIndex("Results.Goals", new[] { "GameWeekId" });
            DropIndex("Results.Goals", new[] { "PlayerId" });
            DropIndex("DreamLeague.ManagerPlayers", new[] { "ManagerId" });
            DropIndex("DreamLeague.ManagerPlayers", new[] { "PlayerId" });
            DropIndex("League.Players", new[] { "TeamId" });
            DropIndex("League.Teams", new[] { "LeagueId" });
            DropIndex("DreamLeague.ManagerGoalKeepers", new[] { "ManagerId" });
            DropIndex("DreamLeague.ManagerGoalKeepers", new[] { "TeamId" });
            DropIndex("DreamLeague.Emails", new[] { "ManagerId" });
            DropIndex("Results.Conceded", new[] { "ManagerId" });
            DropIndex("Results.Conceded", new[] { "GameWeekId" });
            DropIndex("Results.Conceded", new[] { "TeamId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("Results.Goals");
            DropTable("DreamLeague.ManagerPlayers");
            DropTable("League.Players");
            DropTable("League.Leagues");
            DropTable("League.Teams");
            DropTable("DreamLeague.ManagerGoalKeepers");
            DropTable("DreamLeague.Emails");
            DropTable("DreamLeague.Managers");
            DropTable("DreamLeague.GameWeeks");
            DropTable("Results.Conceded");
        }
    }
}
