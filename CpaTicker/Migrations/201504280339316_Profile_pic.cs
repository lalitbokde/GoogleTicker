namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile_pic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfilePic",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProfilePic = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfilePic");
        }
    }
}
