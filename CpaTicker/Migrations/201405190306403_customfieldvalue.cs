namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customfieldvalue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomFieldValues",
                c => new
                    {
                        CustomFieldId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.CustomFieldId, t.CampaignId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.CustomFields", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.CustomFieldId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomFieldValues", "CustomFieldId", "dbo.CustomFields");
            DropForeignKey("dbo.CustomFieldValues", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.CustomFieldValues", new[] { "CustomFieldId" });
            DropIndex("dbo.CustomFieldValues", new[] { "CampaignId" });
            DropTable("dbo.CustomFieldValues");
        }
    }
}
