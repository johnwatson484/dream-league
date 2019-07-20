namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Audit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DreamLeague.Audit",
                c => new
                {
                    AuditId = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    User = c.String(),
                    Area = c.String(),
                    Action = c.String(),
                    GameWeekId = c.Int(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.AuditId);

        }

        public override void Down()
        {
            DropTable("DreamLeague.Audit");
        }
    }
}
