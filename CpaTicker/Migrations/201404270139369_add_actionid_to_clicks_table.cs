namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_actionid_to_clicks_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "ActionId", c => c.Int());
            CreateIndex("dbo.Clicks", "ActionId");
            AddForeignKey("dbo.Clicks", "ActionId", "dbo.Actions", "ActionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clicks", "ActionId", "dbo.Actions");
            DropIndex("dbo.Clicks", new[] { "ActionId" });
            DropColumn("dbo.Clicks", "ActionId");
        }
    }
}
