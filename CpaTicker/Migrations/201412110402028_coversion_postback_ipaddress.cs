namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coversion_postback_ipaddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversions", "Postback_IPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversions", "Postback_IPAddress");
        }
    }
}
