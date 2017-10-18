namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePic : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.UserProfilePic");
        }
    }
}
