namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_campaign_fields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaigns", "OfferUrl");
            DropColumn("dbo.Campaigns", "PreviewUrl");
            DropColumn("dbo.Campaigns", "TrackingType");
            DropColumn("dbo.Campaigns", "RevenueType");
            DropColumn("dbo.Campaigns", "Revenue");
            DropColumn("dbo.Campaigns", "RevenuePercent");
            DropColumn("dbo.Campaigns", "PayoutType");
            DropColumn("dbo.Campaigns", "Payout");
            DropColumn("dbo.Campaigns", "PayoutPercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "PayoutPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Campaigns", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Campaigns", "PayoutType", c => c.Int(nullable: false));
            AddColumn("dbo.Campaigns", "RevenuePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Campaigns", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Campaigns", "RevenueType", c => c.Int(nullable: false));
            AddColumn("dbo.Campaigns", "TrackingType", c => c.Int(nullable: false));
            AddColumn("dbo.Campaigns", "PreviewUrl", c => c.String());
            AddColumn("dbo.Campaigns", "OfferUrl", c => c.String());
        }
    }
}
