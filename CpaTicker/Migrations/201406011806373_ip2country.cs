namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ip2country : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IP2Country",
                c => new
                    {
                        Min = c.Long(nullable: false),
                        Max = c.Long(nullable: false),
                        Code = c.String(maxLength: 2, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Min);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IP2Country");
        }
    }
}
