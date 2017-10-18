namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campaigncountry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignCountries",
                c => new
                    {
                        CampaignId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.Code })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignCountries", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.CampaignCountries", new[] { "CampaignId" });
            DropTable("dbo.CampaignCountries");
        }
    }
}
