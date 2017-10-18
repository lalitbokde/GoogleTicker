namespace CpaTicker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_actDataFields_InConversion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversions", "act_data1", c => c.String());
            AddColumn("dbo.Conversions", "act_data2", c => c.String());
            AddColumn("dbo.Conversions", "act_data3", c => c.String());
            AddColumn("dbo.Conversions", "act_data4", c => c.String());
            AddColumn("dbo.Conversions", "act_data5", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversions", "act_data5");
            DropColumn("dbo.Conversions", "act_data4");
            DropColumn("dbo.Conversions", "act_data3");
            DropColumn("dbo.Conversions", "act_data2");
            DropColumn("dbo.Conversions", "act_data1");
        }
    }
}
