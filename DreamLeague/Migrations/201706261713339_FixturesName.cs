namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixturesName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Cup.CupFixtures", newName: "Fixtures");
        }

        public override void Down()
        {
            RenameTable(name: "Cup.Fixtures", newName: "CupFixtures");
        }
    }
}
