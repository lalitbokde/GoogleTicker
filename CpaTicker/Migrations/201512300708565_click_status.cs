namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "Status", c => c.Int(nullable: false));
            CreateIndex("dbo.Clicks", "Status");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Clicks", new[] { "Status" });
            DropColumn("dbo.Clicks", "Status");
        }
    }
}
