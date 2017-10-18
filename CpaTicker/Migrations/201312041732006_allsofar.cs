namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allsofar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false, identity: true));
        }
    }
}
