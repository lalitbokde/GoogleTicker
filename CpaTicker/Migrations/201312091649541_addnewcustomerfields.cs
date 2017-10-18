namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewcustomerfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CreditCardData", c => c.String());
            AddColumn("dbo.Customers", "MemberSince", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "MemberSince");
            DropColumn("dbo.Customers", "CreditCardData");
        }
    }
}
