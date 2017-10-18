namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeIPs",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        IPAddress = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CustomerId, t.IPAddress });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeIPs");
        }
    }
}
