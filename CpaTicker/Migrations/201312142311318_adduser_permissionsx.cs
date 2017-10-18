namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduser_permissionsx : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Permissions", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Permissions");
        }
    }
}
