namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeinterval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "TimeInterval", c => c.Long(nullable: false, defaultValue: 0));
            AddColumn("dbo.ConversionLogs", "TimeInterval", c => c.Long(nullable: false, defaultValue: 0));
            AddColumn("dbo.Conversions", "TimeInterval", c => c.Long(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversions", "TimeInterval");
            DropColumn("dbo.ConversionLogs", "TimeInterval");
            DropColumn("dbo.Clicks", "TimeInterval");
        }
    }
}
