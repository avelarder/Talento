namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppsettings2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationParameters", "SettingName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationParameters", "SettingName", c => c.String(nullable: false, maxLength: 60));
        }
    }
}
