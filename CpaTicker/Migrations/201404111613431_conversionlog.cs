namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversionlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversionLogs",
                c => new
                    {
                        ConversionLogId = c.Int(nullable: false, identity: true),
                        ConversionDate = c.DateTime(nullable: false),
                        Success = c.Boolean(nullable: false),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.ConversionLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConversionLogs");
        }
    }
}
