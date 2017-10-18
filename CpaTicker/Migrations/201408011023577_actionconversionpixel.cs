namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actionconversionpixel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionConversionPixels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(nullable: false),
                        ConversionPixelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.ConversionPixels", t => t.ConversionPixelId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.ConversionPixelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActionConversionPixels", "ConversionPixelId", "dbo.ConversionPixels");
            DropForeignKey("dbo.ActionConversionPixels", "ActionId", "dbo.Actions");
            DropIndex("dbo.ActionConversionPixels", new[] { "ConversionPixelId" });
            DropIndex("dbo.ActionConversionPixels", new[] { "ActionId" });
            DropTable("dbo.ActionConversionPixels");
        }
    }
}
