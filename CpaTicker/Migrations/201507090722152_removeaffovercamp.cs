namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeaffovercamp : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.OverrideAffiliate");
        }
        
        public override void Down()
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
    }
}
