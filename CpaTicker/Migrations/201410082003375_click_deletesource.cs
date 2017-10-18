namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_deletesource : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clicks", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clicks", "Source", c => c.String());
        }
    }
}
