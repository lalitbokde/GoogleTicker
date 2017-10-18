namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20131106_FirstMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "CustomerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "CustomerId");
        }
    }
}
