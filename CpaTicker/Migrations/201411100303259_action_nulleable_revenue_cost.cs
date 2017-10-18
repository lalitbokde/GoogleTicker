namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action_nulleable_revenue_cost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actions", "PayoutPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "Payout", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "RevenuePercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "Revenue", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Actions", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "RevenuePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "PayoutPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
