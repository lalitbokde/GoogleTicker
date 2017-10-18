namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_userinfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clicks", "UserAgentId");
            DropColumn("dbo.Conversions", "UserAgentId");
            DropTable("dbo.tblUserAgentInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblUserAgentInfoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.String(),
                        IsSmartphone = c.Boolean(nullable: false),
                        IsiOS = c.Boolean(nullable: false),
                        IsAndroid = c.Boolean(nullable: false),
                        OS = c.String(),
                        Browser = c.String(),
                        Device_os = c.String(),
                        Pointing_method = c.String(),
                        Is_tablet = c.Boolean(nullable: false),
                        Model_name = c.String(),
                        Device_os_version = c.String(),
                        Is_wireless_device = c.Boolean(nullable: false),
                        Brand_name = c.String(),
                        Marketing_name = c.String(),
                        Is_assign_phone_number = c.Boolean(nullable: false),
                        Xhtmlmp_mime_type = c.String(),
                        Xhtml_support_level = c.String(),
                        Resolution_height = c.String(),
                        Resolution_width = c.String(),
                        Canvas_support = c.String(),
                        Viewport_width = c.String(),
                        Html_preferred_dtd = c.String(),
                        Isviewport_supported = c.Boolean(nullable: false),
                        Ismobileoptimized = c.Boolean(nullable: false),
                        Isimage_inlining = c.Boolean(nullable: false),
                        Ishandheldfriendly = c.Boolean(nullable: false),
                        Is_smarttv = c.Boolean(nullable: false),
                        Isux_full_desktop = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Conversions", "UserAgentId", c => c.Guid());
            AddColumn("dbo.Clicks", "UserAgentId", c => c.Guid());
        }
    }
}
