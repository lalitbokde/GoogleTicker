namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class log_linenumber_filename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Log", "LineNumber", c => c.Int());
            AddColumn("dbo.Log", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Log", "FileName");
            DropColumn("dbo.Log", "LineNumber");
        }
    }
}
