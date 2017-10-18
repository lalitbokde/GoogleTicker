namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class url_nulleable_revenue_cost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.URLs", "PayoutPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "Payout", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "RevenuePercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "Revenue", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.URLs", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "RevenuePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.URLs", "PayoutPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
