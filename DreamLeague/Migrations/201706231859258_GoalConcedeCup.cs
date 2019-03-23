namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalConcedeCup : DbMigration
    {
        public override void Up()
        {
            AddColumn("Results.Conceded", "CupId", c => c.Int());
            AddColumn("Results.Goals", "CupId", c => c.Int());
            DropColumn("Results.Conceded", "Cup");
            DropColumn("Results.Goals", "Cup");
        }
        
        public override void Down()
        {
            AddColumn("Results.Goals", "Cup", c => c.Boolean(nullable: false));
            AddColumn("Results.Conceded", "Cup", c => c.Boolean(nullable: false));
            DropColumn("Results.Goals", "CupId");
            DropColumn("Results.Conceded", "CupId");
        }
    }
}
