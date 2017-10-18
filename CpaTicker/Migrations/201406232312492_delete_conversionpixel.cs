namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_conversionpixel : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ConversionPixels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ConversionPixels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        AffiliateId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        TrackingType = c.Int(nullable: false),
                        PixelCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
