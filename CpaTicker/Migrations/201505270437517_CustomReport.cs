namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCustomReport",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        ReportName = c.String(),
                        ReportData = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserCustomReport");
        }
    }
}
