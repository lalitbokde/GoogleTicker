namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cp_update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Actions", "ConversionPixelId", "dbo.ConversionPixels");
            DropIndex("dbo.Actions", new[] { "ConversionPixelId" });
            AddColumn("dbo.Actions", "TrackingCode", c => c.String(nullable: false));
            DropColumn("dbo.Actions", "ConversionPixelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Actions", "ConversionPixelId", c => c.Int(nullable: false));
            DropColumn("dbo.Actions", "TrackingCode");
            CreateIndex("dbo.Actions", "ConversionPixelId");
            AddForeignKey("dbo.Actions", "ConversionPixelId", "dbo.ConversionPixels", "Id", cascadeDelete: true);
        }
    }
}
