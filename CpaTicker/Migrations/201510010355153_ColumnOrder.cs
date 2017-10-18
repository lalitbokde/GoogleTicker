namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCustomReport", "ColumnOrder", c => c.String(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCustomReport", "ColumnOrder");
        }
    }
}
