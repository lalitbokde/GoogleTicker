namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tickerelements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TickerElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TickerId = c.Int(nullable: false),
                        CampaignId = c.Int(),
                        AffiliateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Affiliates", t => t.AffiliateId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Tickers", t => t.TickerId, cascadeDelete: true)
                .Index(t => t.AffiliateId)
                .Index(t => t.CampaignId)
                .Index(t => t.TickerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TickerElements", "TickerId", "dbo.Tickers");
            DropForeignKey("dbo.TickerElements", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.TickerElements", "AffiliateId", "dbo.Affiliates");
            DropIndex("dbo.TickerElements", new[] { "TickerId" });
            DropIndex("dbo.TickerElements", new[] { "CampaignId" });
            DropIndex("dbo.TickerElements", new[] { "AffiliateId" });
            DropTable("dbo.TickerElements");
        }
    }
}
