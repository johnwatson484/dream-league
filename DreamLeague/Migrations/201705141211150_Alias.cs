namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alias : DbMigration
    {
        public override void Up()
        {
            AddColumn("DreamLeague.Managers", "Alias", c => c.String());
        }

        public override void Down()
        {
            DropColumn("DreamLeague.Managers", "Alias");
        }
    }
}
