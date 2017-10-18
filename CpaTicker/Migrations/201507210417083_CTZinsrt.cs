namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CTZinsrt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomTimeZone",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false, defaultValue: 0),
                        DisplayName = c.String(),
                        DisplayID = c.String(),
                        offset = c.Int(nullable: false, defaultValue: 0),
                        dstoffset = c.Int(nullable: false, defaultValue: 0),
                        IsdstSupport = c.Boolean(nullable: false, defaultValue: false),
                        dstStart = c.DateTime(),
                        dstEnd = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropTable("dbo.CustomTimeZone");
        }
    }
}
