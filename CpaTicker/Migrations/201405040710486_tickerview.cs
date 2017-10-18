namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tickerview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "View", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "View");
        }
    }
}
