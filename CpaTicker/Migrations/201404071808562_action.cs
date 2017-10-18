namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Actions", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Actions", new[] { "CampaignId" });
            DropTable("dbo.Actions");
        }
    }
}
