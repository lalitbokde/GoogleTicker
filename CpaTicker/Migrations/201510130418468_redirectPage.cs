namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redirectPage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RedirectUrls", "PAGE_Id", "dbo.PAGE");
            DropIndex("dbo.RedirectUrls", new[] { "PAGE_Id" });
            CreateTable(
                "dbo.RedirectPAGEs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PAGEId = c.Int(nullable: false),
                        RedirectPage = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PAGE", t => t.PAGEId, cascadeDelete: true)
                .Index(t => t.PAGEId);
            
            CreateTable(
                "dbo.RedirectTargetPages",
                c => new
                    {
                        Min = c.Long(nullable: false),
                        RedirectPageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Min, t.RedirectPageId })
                .ForeignKey("dbo.IP2Country", t => t.Min, cascadeDelete: true)
                .ForeignKey("dbo.RedirectPAGEs", t => t.RedirectPageId, cascadeDelete: true)
                .Index(t => t.Min)
                .Index(t => t.RedirectPageId);
            
            DropColumn("dbo.RedirectUrls", "PAGE_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RedirectUrls", "PAGE_Id", c => c.Int());
            DropForeignKey("dbo.RedirectTargetPages", "RedirectPageId", "dbo.RedirectPAGEs");
            DropForeignKey("dbo.RedirectTargetPages", "Min", "dbo.IP2Country");
            DropForeignKey("dbo.RedirectPAGEs", "PAGEId", "dbo.PAGE");
            DropIndex("dbo.RedirectTargetPages", new[] { "RedirectPageId" });
            DropIndex("dbo.RedirectTargetPages", new[] { "Min" });
            DropIndex("dbo.RedirectPAGEs", new[] { "PAGEId" });
            DropTable("dbo.RedirectTargetPages");
            DropTable("dbo.RedirectPAGEs");
            CreateIndex("dbo.RedirectUrls", "PAGE_Id");
            AddForeignKey("dbo.RedirectUrls", "PAGE_Id", "dbo.PAGE", "Id");
        }
    }
}
