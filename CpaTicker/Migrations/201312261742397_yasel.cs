namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yasel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "BannerId", c => c.Int(nullable: false));
            AddColumn("dbo.Conversions", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Conversions", "BannerId", c => c.Int(nullable: false));
            AddColumn("dbo.Impressions", "BannerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Impressions", "BannerId");
            DropColumn("dbo.Conversions", "BannerId");
            DropColumn("dbo.Conversions", "Status");
            DropColumn("dbo.Clicks", "BannerId");
        }
    }
}
