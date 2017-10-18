namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Impression_subids : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImpressionSubIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImpressionId = c.Int(nullable: false),
                        SubValue = c.String(),
                        SubIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Impressions", t => t.ImpressionId, cascadeDelete: true)
                .Index(t => t.ImpressionId)
                .Index(t => t.SubIndex);
            
            DropColumn("dbo.Impressions", "Subid1");
            DropColumn("dbo.Impressions", "Subid2");
            DropColumn("dbo.Impressions", "Subid3");
            DropColumn("dbo.Impressions", "Subid4");
            DropColumn("dbo.Impressions", "Subid5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Impressions", "Subid5", c => c.String());
            AddColumn("dbo.Impressions", "Subid4", c => c.String());
            AddColumn("dbo.Impressions", "Subid3", c => c.String());
            AddColumn("dbo.Impressions", "Subid2", c => c.String());
            AddColumn("dbo.Impressions", "Subid1", c => c.String());
            DropForeignKey("dbo.ImpressionSubIds", "ImpressionId", "dbo.Impressions");
            DropIndex("dbo.ImpressionSubIds", new[] { "SubIndex" });
            DropIndex("dbo.ImpressionSubIds", new[] { "ImpressionId" });
            DropTable("dbo.ImpressionSubIds");
        }
    }
}
