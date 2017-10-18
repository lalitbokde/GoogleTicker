namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversion_pixels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversions", "FiredPixels", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversions", "FiredPixels");
        }
    }
}
