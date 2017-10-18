namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_foreign_key_customer_country : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customers", "CountryId");
            AddForeignKey("dbo.Customers", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "CountryId", "dbo.Countries");
            DropIndex("dbo.Customers", new[] { "CountryId" });
        }
    }
}
