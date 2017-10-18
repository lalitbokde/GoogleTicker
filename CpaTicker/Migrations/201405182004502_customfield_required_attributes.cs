namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customfield_required_attributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomFields", "FieldName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomFields", "FieldName", c => c.String());
        }
    }
}
