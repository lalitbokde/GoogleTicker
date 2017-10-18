namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_cookies__conversionLog_campaignid_customerid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "Cookies", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConversionLogs", "CampaignId", c => c.Int());
            AddColumn("dbo.ConversionLogs", "CustomerId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConversionLogs", "CustomerId");
            DropColumn("dbo.ConversionLogs", "CampaignId");
            DropColumn("dbo.Clicks", "Cookies");
        }
    }
}
