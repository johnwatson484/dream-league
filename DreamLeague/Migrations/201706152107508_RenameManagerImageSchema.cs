namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameManagerImageSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.ManagerImages", newSchema: "DreamLeague");
        }
        
        public override void Down()
        {
            MoveTable(name: "DreamLeague.ManagerImages", newSchema: "dbo");
        }
    }
}
