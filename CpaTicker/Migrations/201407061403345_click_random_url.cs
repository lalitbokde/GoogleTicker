namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_random_url : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "Random", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clicks", "Random");
        }
    }
}
