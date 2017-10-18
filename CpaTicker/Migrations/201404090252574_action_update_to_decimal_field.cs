namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action_update_to_decimal_field : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actions", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Actions", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Actions", "Revenue", c => c.Double(nullable: false));
            AlterColumn("dbo.Actions", "Cost", c => c.Double(nullable: false));
        }
    }
}
