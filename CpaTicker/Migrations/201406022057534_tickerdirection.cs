namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tickerdirection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "Direction", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "Direction");
        }
    }
}
