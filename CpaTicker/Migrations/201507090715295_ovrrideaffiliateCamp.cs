namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ovrrideaffiliateCamp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OverrideAffiliate",
                c => new
                    {
                        OverridID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        AffiliateID = c.Int(),
                        PayoutPercent = c.Decimal(precision: 18, scale: 2),
                        PayoutType = c.Int(nullable: false),
                        Payout = c.Decimal(precision: 18, scale: 2),
                        RevenuePercent = c.Decimal(precision: 18, scale: 2),
                        RevenueType = c.Int(nullable: false),
                        Revenue = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OverridID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OverrideAffiliate");
        }
    }
}
