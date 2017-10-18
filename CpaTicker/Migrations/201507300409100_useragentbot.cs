namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class useragentbot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "bot", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Conversions", "bot", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Impressions", "bot", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.Impressions", "bot");
            DropColumn("dbo.Conversions", "bot");
            DropColumn("dbo.Clicks", "bot");
        }
    }
}
