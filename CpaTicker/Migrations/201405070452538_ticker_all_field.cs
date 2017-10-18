namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticker_all_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "All", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "All");
        }
    }
}
