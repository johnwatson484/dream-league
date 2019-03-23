namespace DreamLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveManagerFixture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Cup.CupFixtures", "Manager_ManagerId", "DreamLeague.Managers");
            DropIndex("Cup.CupFixtures", new[] { "Manager_ManagerId" });
            DropColumn("Cup.CupFixtures", "Manager_ManagerId");
        }
        
        public override void Down()
        {
            AddColumn("Cup.CupFixtures", "Manager_ManagerId", c => c.Int());
            CreateIndex("Cup.CupFixtures", "Manager_ManagerId");
            AddForeignKey("Cup.CupFixtures", "Manager_ManagerId", "DreamLeague.Managers", "ManagerId");
        }
    }
}
