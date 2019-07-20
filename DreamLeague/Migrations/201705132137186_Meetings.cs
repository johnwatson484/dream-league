namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Meetings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DreamLeague.Meetings",
                c => new
                {
                    MeetingId = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Location = c.String(),
                    Longitute = c.Double(nullable: false),
                    Latitute = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.MeetingId);

        }

        public override void Down()
        {
            DropTable("DreamLeague.Meetings");
        }
    }
}
