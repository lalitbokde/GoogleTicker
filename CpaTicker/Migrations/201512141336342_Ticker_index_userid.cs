namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ticker_index_userid : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tickers", "UserId");
            AddForeignKey("dbo.Tickers", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickers", "UserId", "dbo.UserProfile");
            DropIndex("dbo.Tickers", new[] { "UserId" });
        }
    }
}
