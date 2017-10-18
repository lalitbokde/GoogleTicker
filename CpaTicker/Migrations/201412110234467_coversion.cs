namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coversion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConversionLogs", "AffiliateId", c => c.Int());
            AddColumn("dbo.ConversionLogs", "TransactionId", c => c.String());
            AddColumn("dbo.ConversionLogs", "Postback", c => c.Boolean());
            AddColumn("dbo.ConversionLogs", "PixelsFound", c => c.Int(nullable: false));
            AddColumn("dbo.ConversionLogs", "Output", c => c.String());
            AddColumn("dbo.Conversions", "Pixel", c => c.DateTime());
            AddColumn("dbo.Conversions", "Postback", c => c.DateTime());
            AddColumn("dbo.Conversions", "StatusDescription", c => c.String());
            DropColumn("dbo.Conversions", "FiredPixels");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conversions", "FiredPixels", c => c.Int(nullable: false));
            DropColumn("dbo.Conversions", "StatusDescription");
            DropColumn("dbo.Conversions", "Postback");
            DropColumn("dbo.Conversions", "Pixel");
            DropColumn("dbo.ConversionLogs", "Output");
            DropColumn("dbo.ConversionLogs", "PixelsFound");
            DropColumn("dbo.ConversionLogs", "Postback");
            DropColumn("dbo.ConversionLogs", "TransactionId");
            DropColumn("dbo.ConversionLogs", "AffiliateId");
        }
    }
}
