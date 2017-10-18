namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        locId = c.Int(nullable: false),
                        country = c.String(maxLength: 2, unicode: false),
                        region = c.String(maxLength: 2, unicode: false),
                        city = c.String(),
                        postalCode = c.String(),
                        latitude = c.String(),
                        longitude = c.String(),
                        metroCode = c.String(),
                        areaCode = c.String(),
                    })
                .PrimaryKey(t => t.locId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
