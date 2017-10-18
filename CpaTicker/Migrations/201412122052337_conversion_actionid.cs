namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversion_actionid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversions", "ActionId", c => c.Int());
            CreateIndex("dbo.Conversions", "ActionId");
            AddForeignKey("dbo.Conversions", "ActionId", "dbo.Actions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conversions", "ActionId", "dbo.Actions");
            DropIndex("dbo.Conversions", new[] { "ActionId" });
            DropColumn("dbo.Conversions", "ActionId");
        }
    }
}
