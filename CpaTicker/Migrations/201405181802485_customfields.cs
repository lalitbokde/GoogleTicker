namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customfields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomFields",
                c => new
                    {
                        CustomFieldId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        FieldName = c.String(),
                    })
                .PrimaryKey(t => t.CustomFieldId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomFields", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomFields", new[] { "CustomerId" });
            DropTable("dbo.CustomFields");
        }
    }
}
