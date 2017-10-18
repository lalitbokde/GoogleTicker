namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class url_conversion_action : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RedirectTargets",
                c => new
                    {
                        Min = c.Long(nullable: false),
                        RedirectUrlId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Min, t.RedirectUrlId })
                .ForeignKey("dbo.IP2Country", t => t.Min, cascadeDelete: true)
                .ForeignKey("dbo.RedirectUrls", t => t.RedirectUrlId, cascadeDelete: true)
                .Index(t => t.Min)
                .Index(t => t.RedirectUrlId);
            
            CreateTable(
                "dbo.RedirectUrls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        URLId = c.Int(nullable: false),
                        RedirectURL = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.URLs", t => t.URLId, cascadeDelete: true)
                .Index(t => t.URLId);
            
            AddColumn("dbo.Actions", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Actions", "Name", c => c.String());
            AddColumn("dbo.Actions", "PayoutPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Actions", "PayoutType", c => c.Int(nullable: false));
            AddColumn("dbo.Actions", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Actions", "RevenuePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Actions", "RevenueType", c => c.Int(nullable: false));
            AddColumn("dbo.Actions", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Actions", "TrackingType", c => c.Int(nullable: false));
            AddColumn("dbo.URLs", "Name", c => c.String());
            AddColumn("dbo.URLs", "PayoutPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.URLs", "PayoutType", c => c.Int(nullable: false));
            AddColumn("dbo.URLs", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.URLs", "RevenuePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.URLs", "RevenueType", c => c.Int(nullable: false));
            AddColumn("dbo.Conversions", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Impressions", "URLId", c => c.Int());
            CreateIndex("dbo.Conversions", "ClickId");
            CreateIndex("dbo.Impressions", "URLId");
            AddForeignKey("dbo.Conversions", "ClickId", "dbo.Clicks", "ClickId", cascadeDelete: true);
            AddForeignKey("dbo.Impressions", "URLId", "dbo.URLs", "Id");
            DropColumn("dbo.Actions", "TrackingCode");
            DropColumn("dbo.URLs", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.URLs", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Actions", "TrackingCode", c => c.String(nullable: false));
            DropForeignKey("dbo.RedirectTargets", "RedirectUrlId", "dbo.RedirectUrls");
            DropForeignKey("dbo.RedirectUrls", "URLId", "dbo.URLs");
            DropForeignKey("dbo.RedirectTargets", "Min", "dbo.IP2Country");
            DropForeignKey("dbo.Impressions", "URLId", "dbo.URLs");
            DropForeignKey("dbo.Conversions", "ClickId", "dbo.Clicks");
            DropIndex("dbo.RedirectTargets", new[] { "RedirectUrlId" });
            DropIndex("dbo.RedirectUrls", new[] { "URLId" });
            DropIndex("dbo.RedirectTargets", new[] { "Min" });
            DropIndex("dbo.Impressions", new[] { "URLId" });
            DropIndex("dbo.Conversions", new[] { "ClickId" });
            DropColumn("dbo.Impressions", "URLId");
            DropColumn("dbo.Conversions", "Type");
            DropColumn("dbo.URLs", "RevenueType");
            DropColumn("dbo.URLs", "RevenuePercent");
            DropColumn("dbo.URLs", "Payout");
            DropColumn("dbo.URLs", "PayoutType");
            DropColumn("dbo.URLs", "PayoutPercent");
            DropColumn("dbo.URLs", "Name");
            DropColumn("dbo.Actions", "TrackingType");
            DropColumn("dbo.Actions", "Revenue");
            DropColumn("dbo.Actions", "RevenueType");
            DropColumn("dbo.Actions", "RevenuePercent");
            DropColumn("dbo.Actions", "Payout");
            DropColumn("dbo.Actions", "PayoutType");
            DropColumn("dbo.Actions", "PayoutPercent");
            DropColumn("dbo.Actions", "Name");
            DropColumn("dbo.Actions", "Type");
            DropTable("dbo.RedirectUrls");
            DropTable("dbo.RedirectTargets");
        }
    }
}
