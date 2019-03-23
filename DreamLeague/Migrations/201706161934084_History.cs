namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class History : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DreamLeague.History",
                c => new
                    {
                        HistoryId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Teams = c.Int(nullable: false),
                        League1 = c.String(),
                        League2 = c.String(),
                        Cup = c.String(),
                        Plate = c.String(),
                    })
                .PrimaryKey(t => t.HistoryId);
            
        }
        
        public override void Down()
        {
            DropTable("DreamLeague.History");
        }
    }
}
