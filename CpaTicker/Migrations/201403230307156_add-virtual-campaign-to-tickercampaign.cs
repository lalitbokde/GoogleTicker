namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvirtualcampaigntotickercampaign : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TickerCampaigns", "CampaignId");
            AddForeignKey("dbo.TickerCampaigns", "CampaignId", "dbo.Campaigns", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TickerCampaigns", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.TickerCampaigns", new[] { "CampaignId" });
        }
    }
}
