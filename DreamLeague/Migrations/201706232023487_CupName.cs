namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CupName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Cup.Groups", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Cup.Groups", "Name", c => c.Int(nullable: false));
        }
    }
}
