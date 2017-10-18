namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        startIpNum = c.Long(nullable: false),
                        endIpNum = c.Long(nullable: false),
                        locId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.startIpNum)
                .ForeignKey("dbo.Locations", t => t.locId, cascadeDelete: true)
                .Index(t => t.locId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blocks", "locId", "dbo.Locations");
            DropIndex("dbo.Blocks", new[] { "locId" });
            DropTable("dbo.Blocks");
        }
    }
}
