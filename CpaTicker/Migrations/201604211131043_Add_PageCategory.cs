namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PageCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PAGECategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PAGE", "PageCategoryId", c => c.Int());
            CreateIndex("dbo.PAGE", "PageCategoryId");
            AddForeignKey("dbo.PAGE", "PageCategoryId", "dbo.PAGECategory", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PAGE", "PageCategoryId", "dbo.PAGECategory");
            DropIndex("dbo.PAGE", new[] { "PageCategoryId" });
            DropColumn("dbo.PAGE", "PageCategoryId");
            DropTable("dbo.PAGECategory");
        }
    }
}
