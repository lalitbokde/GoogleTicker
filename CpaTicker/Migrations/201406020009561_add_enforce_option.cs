namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_enforce_option : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Enforce", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Enforce");
        }
    }
}
