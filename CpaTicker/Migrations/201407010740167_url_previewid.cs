namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class url_previewid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.URLs", "PreviewId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.URLs", "PreviewId");
        }
    }
}
