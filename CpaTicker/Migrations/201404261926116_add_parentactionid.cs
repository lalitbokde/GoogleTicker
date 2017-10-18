namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_parentactionid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actions", "ParentActionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Actions", "ParentActionId");
        }
    }
}
