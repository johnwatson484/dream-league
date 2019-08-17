namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGoalDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("Results.Conceded", "Created", c => c.DateTime(nullable: false));
            AddColumn("Results.Conceded", "CreatedBy", c => c.String());
            AddColumn("Results.Goals", "Created", c => c.DateTime(nullable: false));
            AddColumn("Results.Goals", "CreatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Results.Goals", "CreatedBy");
            DropColumn("Results.Goals", "Created");
            DropColumn("Results.Conceded", "CreatedBy");
            DropColumn("Results.Conceded", "Created");
        }
    }
}
