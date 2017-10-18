namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CookieExpirationInDays_Campaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "CookieExpirationInDays", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "CookieExpirationInDays");
        }
    }
}
