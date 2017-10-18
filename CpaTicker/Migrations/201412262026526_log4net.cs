namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class log4net : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Log",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Date = c.DateTime(nullable: false),
            //            Thread = c.String(),
            //            Level = c.String(),
            //            Logger = c.String(),
            //            Message = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Log");
        }
    }
}
