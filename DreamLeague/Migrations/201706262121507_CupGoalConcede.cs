namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CupGoalConcede : DbMigration
    {
        public override void Up()
        {
            AddColumn("Results.Conceded", "Cup", c => c.Boolean(nullable: false));
            AddColumn("Results.Goals", "Cup", c => c.Boolean(nullable: false));
            DropColumn("Results.Conceded", "CupId");
            DropColumn("Results.Goals", "CupId");
        }
        
        public override void Down()
        {
            AddColumn("Results.Goals", "CupId", c => c.Int());
            AddColumn("Results.Conceded", "CupId", c => c.Int());
            DropColumn("Results.Goals", "Cup");
            DropColumn("Results.Conceded", "Cup");
        }
    }
}
