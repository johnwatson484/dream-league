namespace DreamLeague.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ConcedeSubstitute : DbMigration
    {
        public override void Up()
        {
            AddColumn("Results.Conceded", "Substitute", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("Results.Conceded", "Substitute");
        }
    }
}
