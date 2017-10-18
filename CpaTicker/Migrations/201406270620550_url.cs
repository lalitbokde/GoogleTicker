namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class url : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Clicks", "ActionId", "dbo.Actions");
            DropIndex("dbo.Actions", new[] { "CampaignId" });
            DropIndex("dbo.Clicks", new[] { "ActionId" });
            CreateTable(
                "dbo.URLs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OfferUrl = c.String(),
                        PreviewUrl = c.String(),
                        ParentURLId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            AddColumn("dbo.Clicks", "URLId", c => c.Int());
            CreateIndex("dbo.Clicks", "URLId");
            AddForeignKey("dbo.Clicks", "URLId", "dbo.URLs", "Id");
            DropColumn("dbo.Clicks", "ActionId");
            DropTable("dbo.Actions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OfferUrl = c.String(),
                        PreviewUrl = c.String(),
                        ParentActionId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId);
            
            AddColumn("dbo.Clicks", "ActionId", c => c.Int());
            DropForeignKey("dbo.Clicks", "URLId", "dbo.URLs");
            DropForeignKey("dbo.URLs", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Clicks", new[] { "URLId" });
            DropIndex("dbo.URLs", new[] { "CampaignId" });
            DropColumn("dbo.Clicks", "URLId");
            DropTable("dbo.URLs");
            CreateIndex("dbo.Clicks", "ActionId");
            CreateIndex("dbo.Actions", "CampaignId");
            AddForeignKey("dbo.Clicks", "ActionId", "dbo.Actions", "ActionId");
            AddForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns", "Id", cascadeDelete: true);
        }
    }
}
