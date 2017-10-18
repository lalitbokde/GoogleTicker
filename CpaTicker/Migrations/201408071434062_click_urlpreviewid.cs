namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_urlpreviewid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clicks", "URLId", "dbo.URLs");
            DropIndex("dbo.Clicks", new[] { "URLId" });
            AddColumn("dbo.Clicks", "URLPreviewId", c => c.Int(nullable: false));
            DropColumn("dbo.Clicks", "URLId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clicks", "URLId", c => c.Int());
            DropColumn("dbo.Clicks", "URLPreviewId");
            CreateIndex("dbo.Clicks", "URLId");
            AddForeignKey("dbo.Clicks", "URLId", "dbo.URLs", "Id");
        }
    }
}
