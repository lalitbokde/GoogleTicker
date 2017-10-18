namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revenue_decimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clicks", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Conversions", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Impressions", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Impressions", "Revenue", c => c.Double(nullable: false));
            AlterColumn("dbo.Conversions", "Revenue", c => c.Double(nullable: false));
            AlterColumn("dbo.Clicks", "Revenue", c => c.Double(nullable: false));
        }
    }
}
