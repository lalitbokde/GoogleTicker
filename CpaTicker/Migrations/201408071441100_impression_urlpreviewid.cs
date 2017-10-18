namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class impression_urlpreviewid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Impressions", "URLId", "dbo.URLs");
            DropIndex("dbo.Impressions", new[] { "URLId" });
            AddColumn("dbo.Impressions", "URLPreviewId", c => c.Int(nullable: false));
            DropColumn("dbo.Impressions", "URLId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Impressions", "URLId", c => c.Int());
            DropColumn("dbo.Impressions", "URLPreviewId");
            CreateIndex("dbo.Impressions", "URLId");
            AddForeignKey("dbo.Impressions", "URLId", "dbo.URLs", "Id");
        }
    }
}
