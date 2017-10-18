namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TickerCampaigns",
                c => new
                    {
                        TickerId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TickerId, t.CampaignId });
            
            CreateTable(
                "dbo.Tickers",
                c => new
                    {
                        TickerId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TickerId);
            
            DropTable("dbo.TickerFields");
        }
        
        public override void Down()
        {
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
            
            DropTable("dbo.Tickers");
            DropTable("dbo.TickerCampaigns");
        }
    }
}
