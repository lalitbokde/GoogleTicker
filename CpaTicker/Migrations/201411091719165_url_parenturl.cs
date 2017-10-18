namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class url_parenturl : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.URLs", "ParentURLId");
            AddForeignKey("dbo.URLs", "ParentURLId", "dbo.URLs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.URLs", "ParentURLId", "dbo.URLs");
            DropIndex("dbo.URLs", new[] { "ParentURLId" });
        }
    }
}
