namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversionpixel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversionPixelCampaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversionPixelId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.ConversionPixels", t => t.ConversionPixelId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.ConversionPixelId);
            
            CreateTable(
                "dbo.ConversionPixels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AffiliateId = c.Int(nullable: false),
                        TrackingType = c.Int(nullable: false),
                        PixelCode = c.String(nullable: false),
                        PixelStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Affiliates", t => t.AffiliateId, cascadeDelete: true)
                .Index(t => t.AffiliateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConversionPixelCampaigns", "ConversionPixelId", "dbo.ConversionPixels");
            DropForeignKey("dbo.ConversionPixels", "AffiliateId", "dbo.Affiliates");
            DropForeignKey("dbo.ConversionPixelCampaigns", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.ConversionPixelCampaigns", new[] { "ConversionPixelId" });
            DropIndex("dbo.ConversionPixels", new[] { "AffiliateId" });
            DropIndex("dbo.ConversionPixelCampaigns", new[] { "CampaignId" });
            DropTable("dbo.ConversionPixels");
            DropTable("dbo.ConversionPixelCampaigns");
        }
    }
}
