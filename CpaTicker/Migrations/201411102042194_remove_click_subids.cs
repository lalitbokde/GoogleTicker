namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_click_subids : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clicks", "Subid1");
            DropColumn("dbo.Clicks", "Subid2");
            DropColumn("dbo.Clicks", "Subid3");
            DropColumn("dbo.Clicks", "Subid4");
            DropColumn("dbo.Clicks", "Subid5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clicks", "Subid5", c => c.String());
            AddColumn("dbo.Clicks", "Subid4", c => c.String());
            AddColumn("dbo.Clicks", "Subid3", c => c.String());
            AddColumn("dbo.Clicks", "Subid2", c => c.String());
            AddColumn("dbo.Clicks", "Subid1", c => c.String());
        }
    }
}
