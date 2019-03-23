namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongLat : DbMigration
    {
        public override void Up()
        {
            RenameColumn("DreamLeague.Meetings", "Longitute", "Longitude");
            RenameColumn("DreamLeague.Meetings", "Latitute", "Latitude");            
        }
        
        public override void Down()
        {
            RenameColumn("DreamLeague.Meetings", "Longitude", "Longitute");
            RenameColumn("DreamLeague.Meetings", "Latitude", "Latitute");
        }
    }
}
