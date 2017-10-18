namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_raname_hitid_source : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Clicks", "Source", c => c.String());
            //DropColumn("dbo.Clicks", "HitId");

            RenameColumn("dbo.Clicks", "HitId", "Source");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Clicks", "HitId", c => c.String());
            //DropColumn("dbo.Clicks", "Source");

            RenameColumn("dbo.Clicks", "Source", "HitId");
        }
    }
}
