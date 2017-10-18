namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action_remove_trackingtype : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Actions", "TrackingType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Actions", "TrackingType", c => c.Int(nullable: false));
        }
    }
}
