namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppsettings : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FileBlobs", new[] { "Candidate_Id" });
            DropPrimaryKey("dbo.FileBlobs");
            CreateTable(
                "dbo.ApplicationParameters",
                c => new
                    {
                        ParameterName = c.String(nullable: false, maxLength: 60),
                        ApplicationSettingId = c.Int(nullable: false),
                        ApplicationParameterId = c.Int(nullable: false, identity: true),
                        ParameterValue = c.String(nullable: false, maxLength: 160),
                        CreationDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ParameterName, t.ApplicationSettingId })
                .ForeignKey("dbo.ApplicationSettings", t => t.ApplicationSettingId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationSettingId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationSettings",
                c => new
                    {
                        ApplicationSettingId = c.Int(nullable: false, identity: true),
                        SettingName = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.ApplicationSettingId)
                .Index(t => t.SettingName, unique: true);
            
            AlterColumn("dbo.FileBlobs", "Candidate_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.FileBlobs", new[] { "Id", "Candidate_Id" });
            CreateIndex("dbo.FileBlobs", "Candidate_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationParameters", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationParameters", "ApplicationSettingId", "dbo.ApplicationSettings");
            DropIndex("dbo.FileBlobs", new[] { "Candidate_Id" });
            DropIndex("dbo.ApplicationSettings", new[] { "SettingName" });
            DropIndex("dbo.ApplicationParameters", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationParameters", new[] { "ApplicationSettingId" });
            DropPrimaryKey("dbo.FileBlobs");
            AlterColumn("dbo.FileBlobs", "Candidate_Id", c => c.Int());
            DropTable("dbo.ApplicationSettings");
            DropTable("dbo.ApplicationParameters");
            AddPrimaryKey("dbo.FileBlobs", "Id");
            CreateIndex("dbo.FileBlobs", "Candidate_Id");
        }
    }
}
