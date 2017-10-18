namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log_exception : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Log", "Exception", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Log", "Exception");
        }
    }
}
