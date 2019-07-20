namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ManagerImageRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers");
            DropForeignKey("DreamLeague.ManagerImages", "ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            AddForeignKey("DreamLeague.ManagerImages", "ManagerId", "DreamLeague.Managers", "ManagerId", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("DreamLeague.ManagerImages", "ManagerId", "DreamLeague.Managers");
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("DreamLeague.ManagerImages", "ManagerId", "DreamLeague.Managers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers", "ManagerId");
        }
    }
}
