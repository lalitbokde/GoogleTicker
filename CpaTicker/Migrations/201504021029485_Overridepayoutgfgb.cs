namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Overridepayoutgfgb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OverridePayout",
                c => new
                {
                    CustomerID = c.Int(nullable: false),
                    OverridID = c.Int(nullable: false, identity: true),
                    ActionID = c.Int(nullable: true),
                    AffiliateID = c.Int(nullable: true),
                    CampaignID = c.Int(nullable: true),
                    PayoutPercent = c.Decimal(precision: 18, scale: 2),
                    PayoutType = c.Int(nullable: false),
                    Payout = c.Decimal(precision: 18, scale: 2),

                })
                .PrimaryKey(t => t.OverridID);

        }

        public override void Down()
        {
            DropTable("dbo.OverridePayout");
        }
    }
}
