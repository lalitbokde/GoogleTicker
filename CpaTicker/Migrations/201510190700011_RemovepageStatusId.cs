namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovepageStatusId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PAGE", "StatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PAGE", "StatusId", c => c.Int(nullable: false));
        }
    }
}
