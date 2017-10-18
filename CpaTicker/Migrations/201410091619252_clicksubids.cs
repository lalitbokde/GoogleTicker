namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clicksubids : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClickSubIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClickId = c.Int(nullable: false),
                        SubValue = c.String(),
                        SubIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clicks", t => t.ClickId, cascadeDelete: true)
                .Index(t => t.ClickId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClickSubIds", "ClickId", "dbo.Clicks");
            DropIndex("dbo.ClickSubIds", new[] { "ClickId" });
            DropTable("dbo.ClickSubIds");
        }
    }
}
