namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Affiliates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AffiliateId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Company = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerId = c.Int(nullable: false),
                        BannerDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CampaignName = c.String(nullable: false),
                        Description = c.String(),
                        OfferUrl = c.String(),
                        PreviewUrl = c.String(),
                        TrackingType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(),
                        RevenueType = c.Int(nullable: false),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RevenuePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayoutType = c.Int(nullable: false),
                        Payout = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayoutPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clicks",
                c => new
                    {
                        ClickId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        AffiliateId = c.Int(nullable: false),
                        ClickDate = c.DateTime(nullable: false),
                        UserAgent = c.String(),
                        IPAddress = c.String(),
                        Referrer = c.String(),
                        HitId = c.String(),
                        TransactionId = c.String(),
                        Source = c.String(),
                        Subid1 = c.String(),
                        Subid2 = c.String(),
                        Subid3 = c.String(),
                        Subid4 = c.String(),
                        Subid5 = c.String(),
                        Cost = c.Double(nullable: false),
                        Revenue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ClickId);
            
            CreateTable(
                "dbo.ConversionPixels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        AffiliateId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        TrackingType = c.Int(nullable: false),
                        PixelCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Conversions",
                c => new
                    {
                        ConversionId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        ClickId = c.Int(nullable: false),
                        ConversionDate = c.DateTime(nullable: false),
                        AffiliateId = c.Int(nullable: false),
                        UserAgent = c.String(),
                        IPAddress = c.String(),
                        Referrer = c.String(),
                        TransactionId = c.String(),
                        Cost = c.Double(nullable: false),
                        Revenue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ConversionId);
            
            CreateTable(
                "dbo.CustomerDomains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        DomainId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        AccountId = c.String(),
                        CompanyName = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        APIKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Domains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DomainName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Impressions",
                c => new
                    {
                        ImpressionId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        AffiliateId = c.Int(nullable: false),
                        ImpressionDate = c.DateTime(nullable: false),
                        UserAgent = c.String(),
                        IPAddress = c.String(),
                        Referrer = c.String(),
                        Source = c.String(),
                        Subid1 = c.String(),
                        Subid2 = c.String(),
                        Subid3 = c.String(),
                        Subid4 = c.String(),
                        Subid5 = c.String(),
                        Cost = c.Double(nullable: false),
                        Revenue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ImpressionId);
            
            CreateTable(
                "dbo.TickerFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        FieldNames = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        AffiliateId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfile");
            DropTable("dbo.TickerFields");
            DropTable("dbo.Impressions");
            DropTable("dbo.Domains");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerDomains");
            DropTable("dbo.Conversions");
            DropTable("dbo.ConversionPixels");
            DropTable("dbo.Clicks");
            DropTable("dbo.Campaigns");
            DropTable("dbo.Banners");
            DropTable("dbo.Affiliates");
        }
    }
}
