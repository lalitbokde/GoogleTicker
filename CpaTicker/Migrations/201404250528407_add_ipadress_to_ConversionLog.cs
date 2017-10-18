namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_ipadress_to_ConversionLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConversionLogs", "IPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConversionLogs", "IPAddress");
        }
    }
}
