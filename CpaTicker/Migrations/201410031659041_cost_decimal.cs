namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cost_decimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clicks", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Conversions", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Impressions", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Impressions", "Cost", c => c.Double(nullable: false));
            AlterColumn("dbo.Conversions", "Cost", c => c.Double(nullable: false));
            AlterColumn("dbo.Clicks", "Cost", c => c.Double(nullable: false));
        }
    }
}
