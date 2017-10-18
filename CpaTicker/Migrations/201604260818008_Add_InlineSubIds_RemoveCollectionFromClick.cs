namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_InlineSubIds_RemoveCollectionFromClick : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "SubId1", c => c.String());
            AddColumn("dbo.Clicks", "SubId2", c => c.String());
            AddColumn("dbo.Clicks", "SubId3", c => c.String());
            AddColumn("dbo.Clicks", "SubId4", c => c.String());
            AddColumn("dbo.Clicks", "SubId5", c => c.String());
            AddColumn("dbo.Clicks", "SubId6", c => c.String());
            AddColumn("dbo.Clicks", "SubId7", c => c.String());
            AddColumn("dbo.Clicks", "SubId8", c => c.String());
            AddColumn("dbo.Clicks", "SubId9", c => c.String());
            AddColumn("dbo.Clicks", "SubId10", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clicks", "SubId10");
            DropColumn("dbo.Clicks", "SubId9");
            DropColumn("dbo.Clicks", "SubId8");
            DropColumn("dbo.Clicks", "SubId7");
            DropColumn("dbo.Clicks", "SubId6");
            DropColumn("dbo.Clicks", "SubId5");
            DropColumn("dbo.Clicks", "SubId4");
            DropColumn("dbo.Clicks", "SubId3");
            DropColumn("dbo.Clicks", "SubId2");
            DropColumn("dbo.Clicks", "SubId1");
        }
    }
}
