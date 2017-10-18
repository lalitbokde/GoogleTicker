namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerlevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Level", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Level");
        }
    }
}
