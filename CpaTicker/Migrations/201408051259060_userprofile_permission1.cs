namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userprofile_permission1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Permissions1", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Permissions1");
        }
    }
}
