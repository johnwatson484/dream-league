namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class IdentityManagerId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Manager_ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.AspNetUsers", new[] { "Manager_ManagerId" });
            RenameColumn(table: "dbo.AspNetUsers", name: "Manager_ManagerId", newName: "ManagerId");
            AlterColumn("dbo.AspNetUsers", "ManagerId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers", "ManagerId", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            AlterColumn("dbo.AspNetUsers", "ManagerId", c => c.Int());
            RenameColumn(table: "dbo.AspNetUsers", name: "ManagerId", newName: "Manager_ManagerId");
            CreateIndex("dbo.AspNetUsers", "Manager_ManagerId");
            AddForeignKey("dbo.AspNetUsers", "Manager_ManagerId", "DreamLeague.Managers", "ManagerId");
        }
    }
}
