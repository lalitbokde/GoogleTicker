namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tickersettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TickerSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        BackgroundColor = c.String(),
                        CampaignColor = c.String(),
                        ImpressionColor = c.String(),
                        ClickColor = c.String(),
                        ConversionColor = c.String(),
                        CostColor = c.String(),
                        RevenueColor = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TickerSettings", "UserId", "dbo.UserProfile");
            DropIndex("dbo.TickerSettings", new[] { "UserId" });
            DropTable("dbo.TickerSettings");
        }
    }
}
