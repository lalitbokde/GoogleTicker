namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CTZremoves : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CustomTimeZone");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomTimeZone",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        DisplayName = c.String(),
                        DisplayID = c.String(),
                        offset = c.Int(nullable: false),
                        dstoffset = c.Int(nullable: false),
                        IsdstSupport = c.Boolean(nullable: false),
                        dstStart = c.DateTime(),
                        dstEnd = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
