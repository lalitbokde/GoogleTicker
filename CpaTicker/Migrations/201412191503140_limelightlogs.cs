namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limelightlogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LimeLightLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Response = c.String(),
                        Request = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LimeLightLogs");
        }
    }
}
