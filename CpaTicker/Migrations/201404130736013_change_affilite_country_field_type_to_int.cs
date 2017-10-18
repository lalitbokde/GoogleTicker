namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_affilite_country_field_type_to_int : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Affiliates", "CountryId", c => c.Int());
            CreateIndex("dbo.Affiliates", "CountryId");
            AddForeignKey("dbo.Affiliates", "CountryId", "dbo.Countries", "Id");
            DropColumn("dbo.Affiliates", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Affiliates", "Country", c => c.String());
            DropForeignKey("dbo.Affiliates", "CountryId", "dbo.Countries");
            DropIndex("dbo.Affiliates", new[] { "CountryId" });
            DropColumn("dbo.Affiliates", "CountryId");
        }
    }
}
