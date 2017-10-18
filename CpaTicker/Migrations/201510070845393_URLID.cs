namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class URLID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "URLId", c => c.Int(nullable: false , defaultValue:0));
            AddColumn("dbo.Impressions", "URLId", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Impressions", "URLId");
            DropColumn("dbo.Clicks", "URLId");
        }
    }
}
