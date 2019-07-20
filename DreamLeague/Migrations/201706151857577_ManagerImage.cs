namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ManagerImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagerImages",
                c => new
                {
                    ManagerId = c.Int(nullable: false),
                    Image = c.Binary(),
                })
                .PrimaryKey(t => t.ManagerId)
                .ForeignKey("DreamLeague.Managers", t => t.ManagerId)
                .Index(t => t.ManagerId);

            AddColumn("DreamLeague.Managers", "AllowImage", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.ManagerImages", "ManagerId", "DreamLeague.Managers");
            DropIndex("dbo.ManagerImages", new[] { "ManagerId" });
            DropColumn("DreamLeague.Managers", "AllowImage");
            DropTable("dbo.ManagerImages");
        }
    }
}
