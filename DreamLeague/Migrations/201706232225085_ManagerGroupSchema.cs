namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerGroupSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.ManagerGroups", newSchema: "Cup");
        }
        
        public override void Down()
        {
            MoveTable(name: "Cup.ManagerGroups", newSchema: "dbo");
        }
    }
}
