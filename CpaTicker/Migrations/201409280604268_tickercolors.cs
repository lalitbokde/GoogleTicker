namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tickercolors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "BackgroundColor", c => c.String());
            AddColumn("dbo.Tickers", "CampaignColor", c => c.String());
            AddColumn("dbo.Tickers", "ImpressionColor", c => c.String());
            AddColumn("dbo.Tickers", "ClickColor", c => c.String());
            AddColumn("dbo.Tickers", "ConversionColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "ConversionColor");
            DropColumn("dbo.Tickers", "ClickColor");
            DropColumn("dbo.Tickers", "ImpressionColor");
            DropColumn("dbo.Tickers", "CampaignColor");
            DropColumn("dbo.Tickers", "BackgroundColor");
        }
    }
}
