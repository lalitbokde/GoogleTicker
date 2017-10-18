namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userprofile_orderid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "OrderId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "OrderId");
        }
    }
}
