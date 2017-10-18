namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customer_transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "TransactionIdType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "TransactionIdType");
        }
    }
}
