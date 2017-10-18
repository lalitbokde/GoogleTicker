namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Block_endIp_index : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Blocks", new[] { "endIpNum" });
            //CreateIndex("dbo.ActionConversionPixels", "ActionId");
            //CreateIndex("dbo.ActionConversionPixels", "ConversionPixelId");
            //CreateIndex("dbo.Actions", "CampaignId");
            //CreateIndex("dbo.CampaignCountries", "CampaignId");
            //CreateIndex("dbo.CustomFieldValues", "CustomFieldId");
            //CreateIndex("dbo.CustomFieldValues", "CampaignId");
            //CreateIndex("dbo.CustomFields", "CustomerId");
            //CreateIndex("dbo.Customers", "CountryId");
            //CreateIndex("dbo.URLs", "CampaignId");
            //CreateIndex("dbo.URLs", "ParentURLId");
            //CreateIndex("dbo.RedirectUrls", "URLId");
            //CreateIndex("dbo.RedirectTargets", "Min");
            //CreateIndex("dbo.RedirectTargets", "RedirectUrlId");
            //CreateIndex("dbo.ConversionPixels", "AffiliateId");
            //CreateIndex("dbo.Affiliates", "CountryId");
            //CreateIndex("dbo.ConversionPixelCampaigns", "ConversionPixelId");
            //CreateIndex("dbo.ConversionPixelCampaigns", "CampaignId");
            CreateIndex("dbo.Blocks", "endIpNum", unique: true);
            //CreateIndex("dbo.Blocks", "locId");
            //CreateIndex("dbo.Clicks", "RedirectUrlId");
            //CreateIndex("dbo.ClickSubIds", "ClickId");
            //CreateIndex("dbo.Conversions", "ClickId");
            //CreateIndex("dbo.Conversions", "ActionId");
            //CreateIndex("dbo.CustomerDomains", "DomainId");
            //CreateIndex("dbo.Orders", "CustomerId");
            //CreateIndex("dbo.TickerCampaigns", "CampaignId");
            //CreateIndex("dbo.TickerElements", "TickerId");
            //CreateIndex("dbo.TickerElements", "CampaignId");
            //CreateIndex("dbo.TickerElements", "AffiliateId");
            //CreateIndex("dbo.TickerSettings", "UserId");
            //CreateIndex("dbo.UserHiddenCampaigns", "UserId");
            //CreateIndex("dbo.UserHiddenCampaigns", "CampaignId");
            //CreateIndex("dbo.Transactions", "OrderId");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Transactions", new[] { "OrderId" });
            //DropIndex("dbo.UserHiddenCampaigns", new[] { "CampaignId" });
            //DropIndex("dbo.UserHiddenCampaigns", new[] { "UserId" });
            //DropIndex("dbo.TickerSettings", new[] { "UserId" });
            //DropIndex("dbo.TickerElements", new[] { "AffiliateId" });
            //DropIndex("dbo.TickerElements", new[] { "CampaignId" });
            //DropIndex("dbo.TickerElements", new[] { "TickerId" });
            //DropIndex("dbo.TickerCampaigns", new[] { "CampaignId" });
            //DropIndex("dbo.Orders", new[] { "CustomerId" });
            //DropIndex("dbo.CustomerDomains", new[] { "DomainId" });
            //DropIndex("dbo.Conversions", new[] { "ActionId" });
            //DropIndex("dbo.Conversions", new[] { "ClickId" });
            //DropIndex("dbo.ClickSubIds", new[] { "ClickId" });
            //DropIndex("dbo.Clicks", new[] { "RedirectUrlId" });
            //DropIndex("dbo.Blocks", new[] { "locId" });
            DropIndex("dbo.Blocks", new[] { "endIpNum" });
            //DropIndex("dbo.ConversionPixelCampaigns", new[] { "CampaignId" });
            //DropIndex("dbo.ConversionPixelCampaigns", new[] { "ConversionPixelId" });
            //DropIndex("dbo.Affiliates", new[] { "CountryId" });
            //DropIndex("dbo.ConversionPixels", new[] { "AffiliateId" });
            //DropIndex("dbo.RedirectTargets", new[] { "RedirectUrlId" });
            //DropIndex("dbo.RedirectTargets", new[] { "Min" });
            //DropIndex("dbo.RedirectUrls", new[] { "URLId" });
            //DropIndex("dbo.URLs", new[] { "ParentURLId" });
            //DropIndex("dbo.URLs", new[] { "CampaignId" });
            //DropIndex("dbo.Customers", new[] { "CountryId" });
            //DropIndex("dbo.CustomFields", new[] { "CustomerId" });
            //DropIndex("dbo.CustomFieldValues", new[] { "CampaignId" });
            //DropIndex("dbo.CustomFieldValues", new[] { "CustomFieldId" });
            //DropIndex("dbo.CampaignCountries", new[] { "CampaignId" });
            //DropIndex("dbo.Actions", new[] { "CampaignId" });
            //DropIndex("dbo.ActionConversionPixels", new[] { "ConversionPixelId" });
            //DropIndex("dbo.ActionConversionPixels", new[] { "ActionId" });
            CreateIndex("dbo.Blocks", "endIpNum");
        }
    }
}
