namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class IdentityManagerIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            AlterColumn("dbo.AspNetUsers", "ManagerId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers", "ManagerId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            AlterColumn("dbo.AspNetUsers", "ManagerId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers", "ManagerId", cascadeDelete: true);
        }
    }
}
