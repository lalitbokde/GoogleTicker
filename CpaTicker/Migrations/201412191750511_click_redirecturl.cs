namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_redirecturl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "RedirectUrlId", c => c.Int());
            CreateIndex("dbo.Clicks", "RedirectUrlId");
            AddForeignKey("dbo.Clicks", "RedirectUrlId", "dbo.RedirectUrls", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clicks", "RedirectUrlId", "dbo.RedirectUrls");
            DropIndex("dbo.Clicks", new[] { "RedirectUrlId" });
            DropColumn("dbo.Clicks", "RedirectUrlId");
        }
    }
}
