namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeclicks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeClicks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClickId = c.Int(nullable: false ,defaultValue:0),
                        Campaign = c.Int(nullable: false, defaultValue: 0),
                        URL = c.Int(nullable: false, defaultValue: 0),
                        SubID = c.Int(nullable: false, defaultValue: 0),
                        Agent = c.Int(nullable: false, defaultValue: 0),
                        Click = c.Int(nullable: false, defaultValue: 0),
                        Cookie = c.Int(nullable: false, defaultValue: 0),
                        total = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TimeClicks");
        }
    }
}
