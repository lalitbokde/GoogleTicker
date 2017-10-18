namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action_cp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ConversionPixelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.ConversionPixels", t => t.ConversionPixelId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.ConversionPixelId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Actions", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Actions", "ConversionPixelId", "dbo.ConversionPixels");
            DropForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Actions", new[] { "UserId" });
            DropIndex("dbo.Actions", new[] { "ConversionPixelId" });
            DropIndex("dbo.Actions", new[] { "CampaignId" });
            DropTable("dbo.Actions");
        }
    }
}
