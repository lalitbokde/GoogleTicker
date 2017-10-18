namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cp_remove_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConversionPixelCampaigns", "PixelStatus", c => c.Int(nullable: false));
            DropColumn("dbo.ConversionPixels", "PixelStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConversionPixels", "PixelStatus", c => c.Int(nullable: false));
            DropColumn("dbo.ConversionPixelCampaigns", "PixelStatus");
        }
    }
}
