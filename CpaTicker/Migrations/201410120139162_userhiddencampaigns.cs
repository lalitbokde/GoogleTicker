namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userhiddencampaigns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserHiddenCampaigns",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CampaignId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserHiddenCampaigns", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.UserHiddenCampaigns", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.UserHiddenCampaigns", new[] { "UserId" });
            DropIndex("dbo.UserHiddenCampaigns", new[] { "CampaignId" });
            DropTable("dbo.UserHiddenCampaigns");
        }
    }
}
