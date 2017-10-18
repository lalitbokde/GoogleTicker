namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticker_speed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "Speed", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "Speed");
        }
    }
}
