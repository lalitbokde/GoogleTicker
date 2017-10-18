namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class click_conv_imp_country : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clicks", "Country", c => c.String());
            AddColumn("dbo.Conversions", "Country", c => c.String());
            AddColumn("dbo.Impressions", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Impressions", "Country");
            DropColumn("dbo.Conversions", "Country");
            DropColumn("dbo.Clicks", "Country");
        }
    }
}
