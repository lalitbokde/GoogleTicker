namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_useragent_column : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clicks", "UserAgentId");
            DropColumn("dbo.Conversions", "UserAgentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conversions", "UserAgentId", c => c.Long(nullable: false));
            AddColumn("dbo.Clicks", "UserAgentId", c => c.Long(nullable: false));
        }
    }
}
