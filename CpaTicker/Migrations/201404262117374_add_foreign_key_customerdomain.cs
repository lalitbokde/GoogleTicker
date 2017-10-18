namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_foreign_key_customerdomain : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomerDomains", "DomainId");
            AddForeignKey("dbo.CustomerDomains", "DomainId", "dbo.Domains", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerDomains", "DomainId", "dbo.Domains");
            DropIndex("dbo.CustomerDomains", new[] { "DomainId" });
        }
    }
}
