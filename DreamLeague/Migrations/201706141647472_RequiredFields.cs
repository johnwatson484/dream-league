namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DreamLeague.Managers", "Name", c => c.String(nullable: false));
            AlterColumn("League.Teams", "Name", c => c.String(nullable: false));
            AlterColumn("League.Leagues", "Name", c => c.String(nullable: false));
            AlterColumn("League.Players", "LastName", c => c.String(nullable: false));
            AlterColumn("DreamLeague.Meetings", "Location", c => c.String(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("DreamLeague.Meetings", "Location", c => c.String());
            AlterColumn("League.Players", "LastName", c => c.String());
            AlterColumn("League.Leagues", "Name", c => c.String());
            AlterColumn("League.Teams", "Name", c => c.String());
            AlterColumn("DreamLeague.Managers", "Name", c => c.String());
        }
    }
}
