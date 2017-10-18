namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loginhistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.loginhistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Date = c.DateTime(nullable: false),
                        UserAgent = c.String(),
                        IPAddress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.loginhistory");
        }
    }
}
