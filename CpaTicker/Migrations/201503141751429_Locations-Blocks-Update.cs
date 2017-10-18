namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationsBlocksUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blocks", "locId", "dbo.Locations");
            DropIndex("dbo.Blocks", new[] { "locId" });
            AddColumn("dbo.Blocks", "registered_country_geoname_id", c => c.Int());
            AddColumn("dbo.Blocks", "represented_country_geoname_id", c => c.Int());
            AddColumn("dbo.Blocks", "is_anonymous_proxy", c => c.Boolean(nullable: false));
            AddColumn("dbo.Blocks", "is_satellite_provider", c => c.Boolean(nullable: false));
            AddColumn("dbo.Blocks", "postal_code", c => c.String());
            AddColumn("dbo.Blocks", "latitude", c => c.String());
            AddColumn("dbo.Blocks", "longitude", c => c.String());
            AddColumn("dbo.Locations", "locale_code", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Locations", "continent_code", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Locations", "continent_name", c => c.String());
            AddColumn("dbo.Locations", "country_iso_code", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Locations", "country_name", c => c.String());
            AddColumn("dbo.Locations", "subdivision_1_iso_code", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.Locations", "subdivision_1_name", c => c.String());
            AddColumn("dbo.Locations", "subdivision_2_iso_code", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.Locations", "subdivision_2_name", c => c.String());
            AddColumn("dbo.Locations", "city_name", c => c.String());
            AddColumn("dbo.Locations", "metro_code", c => c.String());
            AddColumn("dbo.Locations", "time_zone", c => c.String());
            AlterColumn("dbo.Blocks", "locId", c => c.Int());
            CreateIndex("dbo.Blocks", "locId");
            CreateIndex("dbo.Blocks", "registered_country_geoname_id");
            CreateIndex("dbo.Blocks", "represented_country_geoname_id");
            AddForeignKey("dbo.Blocks", "registered_country_geoname_id", "dbo.Locations", "locId");
            AddForeignKey("dbo.Blocks", "represented_country_geoname_id", "dbo.Locations", "locId");
            AddForeignKey("dbo.Blocks", "locId", "dbo.Locations", "locId");
            DropColumn("dbo.Locations", "country");
            DropColumn("dbo.Locations", "region");
            DropColumn("dbo.Locations", "city");
            DropColumn("dbo.Locations", "postalCode");
            DropColumn("dbo.Locations", "latitude");
            DropColumn("dbo.Locations", "longitude");
            DropColumn("dbo.Locations", "metroCode");
            DropColumn("dbo.Locations", "areaCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "areaCode", c => c.String());
            AddColumn("dbo.Locations", "metroCode", c => c.String());
            AddColumn("dbo.Locations", "longitude", c => c.String());
            AddColumn("dbo.Locations", "latitude", c => c.String());
            AddColumn("dbo.Locations", "postalCode", c => c.String());
            AddColumn("dbo.Locations", "city", c => c.String());
            AddColumn("dbo.Locations", "region", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Locations", "country", c => c.String(maxLength: 2, unicode: false));
            DropForeignKey("dbo.Blocks", "locId", "dbo.Locations");
            DropForeignKey("dbo.Blocks", "represented_country_geoname_id", "dbo.Locations");
            DropForeignKey("dbo.Blocks", "registered_country_geoname_id", "dbo.Locations");
            DropIndex("dbo.Blocks", new[] { "represented_country_geoname_id" });
            DropIndex("dbo.Blocks", new[] { "registered_country_geoname_id" });
            DropIndex("dbo.Blocks", new[] { "locId" });
            AlterColumn("dbo.Blocks", "locId", c => c.Int(nullable: false));
            DropColumn("dbo.Locations", "time_zone");
            DropColumn("dbo.Locations", "metro_code");
            DropColumn("dbo.Locations", "city_name");
            DropColumn("dbo.Locations", "subdivision_2_name");
            DropColumn("dbo.Locations", "subdivision_2_iso_code");
            DropColumn("dbo.Locations", "subdivision_1_name");
            DropColumn("dbo.Locations", "subdivision_1_iso_code");
            DropColumn("dbo.Locations", "country_name");
            DropColumn("dbo.Locations", "country_iso_code");
            DropColumn("dbo.Locations", "continent_name");
            DropColumn("dbo.Locations", "continent_code");
            DropColumn("dbo.Locations", "locale_code");
            DropColumn("dbo.Blocks", "longitude");
            DropColumn("dbo.Blocks", "latitude");
            DropColumn("dbo.Blocks", "postal_code");
            DropColumn("dbo.Blocks", "is_satellite_provider");
            DropColumn("dbo.Blocks", "is_anonymous_proxy");
            DropColumn("dbo.Blocks", "represented_country_geoname_id");
            DropColumn("dbo.Blocks", "registered_country_geoname_id");
            CreateIndex("dbo.Blocks", "locId");
            AddForeignKey("dbo.Blocks", "locId", "dbo.Locations", "locId", cascadeDelete: true);
        }
    }
}
