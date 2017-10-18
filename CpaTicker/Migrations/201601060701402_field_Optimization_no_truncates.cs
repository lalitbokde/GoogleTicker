namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class field_Optimization_no_truncates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clicks", "UserAgent", c => c.String(maxLength: 2500));
            AlterColumn("dbo.Clicks", "IPAddress", c => c.String(maxLength: 39, unicode: false));
            AlterColumn("dbo.Clicks", "Source", c => c.String(maxLength: 255));
            AlterColumn("dbo.Clicks", "TransactionId", c => c.String(maxLength: 36, fixedLength: true, unicode: false));
            AlterColumn("dbo.Clicks", "Country", c => c.String(maxLength: 2, fixedLength: true, unicode: false));
            AlterColumn("dbo.ClickSubIds", "SubValue", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Conversions", "UserAgent", c => c.String(maxLength: 2500));
            AlterColumn("dbo.Conversions", "IPAddress", c => c.String(maxLength: 39, unicode: false));
            AlterColumn("dbo.Conversions", "TransactionId", c => c.String(maxLength: 36, fixedLength: true, unicode: false));
            AlterColumn("dbo.Conversions", "StatusDescription", c => c.String(maxLength: 2500));
            AlterColumn("dbo.Conversions", "Postback_IPAddress", c => c.String(maxLength: 39, unicode: false));
            AlterColumn("dbo.Conversions", "Country", c => c.String(maxLength: 2, fixedLength: true, unicode: false));
            AlterColumn("dbo.Impressions", "UserAgent", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Impressions", "IPAddress", c => c.String(maxLength: 39, unicode: false));
            AlterColumn("dbo.Impressions", "Source", c => c.String(maxLength: 255));
            AlterColumn("dbo.Impressions", "Country", c => c.String(maxLength: 2, fixedLength: true, unicode: false));
            AlterColumn("dbo.ImpressionSubIds", "SubValue", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ImpressionSubIds", "SubValue", c => c.String());
            AlterColumn("dbo.Impressions", "Country", c => c.String());
            AlterColumn("dbo.Impressions", "Source", c => c.String());
            AlterColumn("dbo.Impressions", "IPAddress", c => c.String());
            AlterColumn("dbo.Impressions", "UserAgent", c => c.String());
            AlterColumn("dbo.Conversions", "Country", c => c.String());
            AlterColumn("dbo.Conversions", "Postback_IPAddress", c => c.String());
            AlterColumn("dbo.Conversions", "StatusDescription", c => c.String());
            AlterColumn("dbo.Conversions", "TransactionId", c => c.String());
            AlterColumn("dbo.Conversions", "IPAddress", c => c.String());
            AlterColumn("dbo.Conversions", "UserAgent", c => c.String());
            AlterColumn("dbo.ClickSubIds", "SubValue", c => c.String());
            AlterColumn("dbo.Clicks", "Country", c => c.String());
            AlterColumn("dbo.Clicks", "TransactionId", c => c.String());
            AlterColumn("dbo.Clicks", "Source", c => c.String());
            AlterColumn("dbo.Clicks", "IPAddress", c => c.String());
            AlterColumn("dbo.Clicks", "UserAgent", c => c.String());
        }
    }
}
