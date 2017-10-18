namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_action : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Actions", new[] { "CampaignId" });
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
                        Cost = c.Double(nullable: false),
                        Revenue = c.Double(nullable: false),
                        OfferUrl = c.String(),
                        PreviewUrl = c.String(),
                    })
                .PrimaryKey(t => t.ActionId);
            
            CreateIndex("dbo.Actions", "CampaignId");
            AddForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns", "Id", cascadeDelete: true);
        }
    }
}
