namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class StatusID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PAGE", "StatusId", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.PAGE", "StatusId");
        }
    }
}
