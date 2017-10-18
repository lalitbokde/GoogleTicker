namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Banners", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Banners", "Image", c => c.Binary(nullable: false));
        }
    }
}
