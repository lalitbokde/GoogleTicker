namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_useragent_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "UserAgentId", c => c.Long(nullable: false, defaultValue: 0));
            AddColumn("dbo.Conversions", "UserAgentId", c => c.Long(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversions", "UserAgentId");
            DropColumn("dbo.Clicks", "UserAgentId");
        }
    }
}
