namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class action_remove_userid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Actions", "UserId", "dbo.UserProfile");
            DropIndex("dbo.Actions", new[] { "UserId" });
            DropColumn("dbo.Actions", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Actions", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Actions", "UserId");
            AddForeignKey("dbo.Actions", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
        }
    }
}
