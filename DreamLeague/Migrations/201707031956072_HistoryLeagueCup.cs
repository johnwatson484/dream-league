namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HistoryLeagueCup : DbMigration
    {
        public override void Up()
        {
            AddColumn("DreamLeague.History", "LeagueCup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DreamLeague.History", "LeagueCup");
        }
    }
}
