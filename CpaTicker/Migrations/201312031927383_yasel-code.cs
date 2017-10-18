namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yaselcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "TimeZone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "TimeZone");
        }
    }
}
