namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_Profilepic : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserProfilePic");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProfilePic",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        ProfilePic = c.Binary(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
    }
}
