namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TeamAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("League.Teams", "Alias", c => c.String());
        }

        public override void Down()
        {
            DropColumn("League.Teams", "Alias");
        }
    }
}
