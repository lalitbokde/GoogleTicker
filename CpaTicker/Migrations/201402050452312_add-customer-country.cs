namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcustomercountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CountryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CountryId");
        }
    }
}
