namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationSettings",
                c => new
                    {
                        ParameterName = c.String(nullable: false, maxLength: 60),
                        SettingName = c.String(nullable: false, maxLength: 60),
                        ApplicationSettingId = c.Int(nullable: false, identity: true),
                        ParameterValue = c.String(nullable: false, maxLength: 160),
                        CreationDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ParameterName, t.SettingName })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImageProfile = c.Binary(storeType: "image"),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Competencies = c.String(nullable: false, maxLength: 300),
                        Description = c.String(nullable: false, maxLength: 300),
                        CreatedOn = c.DateTime(),
                        CreatedBy_Id = c.String(maxLength: 128),
                        IsTcsEmployee = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CandidateId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.FileBlobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        Blob = c.Binary(),
                        Candidate_CandidateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_CandidateId)
                .Index(t => t.Candidate_CandidateId);
            
            CreateTable(
                "dbo.PositionCandidates",
                c => new
                    {
                        PositionID = c.Int(nullable: false),
                        CandidateID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PositionID, t.CandidateID })
                .ForeignKey("dbo.Candidates", t => t.CandidateID)
                .ForeignKey("dbo.Positions", t => t.PositionID)
                .Index(t => t.PositionID)
                .Index(t => t.CandidateID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 500),
                        CreationDate = c.DateTime(nullable: false),
                        Area = c.String(nullable: false, maxLength: 20),
                        EngagementManager = c.String(nullable: false, maxLength: 50),
                        PortfolioManager_Id = c.String(nullable: false, maxLength: 128),
                        RGS = c.String(),
                        Status = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        LastOpenedBy_Id = c.String(maxLength: 128),
                        LastCancelledBy_Id = c.String(maxLength: 128),
                        LastClosedBy_Id = c.String(maxLength: 128),
                        LastOpenedDate = c.DateTime(),
                        LastCancelledDate = c.DateTime(),
                        LastClosedDate = c.DateTime(),
                        OpenStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastCancelledBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.LastClosedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.LastOpenedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PortfolioManager_Id)
                .Index(t => t.PortfolioManager_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.LastOpenedBy_Id)
                .Index(t => t.LastCancelledBy_Id)
                .Index(t => t.LastClosedBy_Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Action = c.Int(nullable: false),
                        PreviousStatus = c.Int(nullable: false),
                        ActualStatus = c.Int(nullable: false),
                        Position_PositionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.Position_PositionId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Position_PositionId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Position_PositionId = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Positions", t => t.Position_PositionId)
                .Index(t => t.Position_PositionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TechnicalInterviews",
                c => new
                    {
                        TechnicalInterviewId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IsAccepted = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 500),
                        InterviewerId = c.String(nullable: false, maxLength: 10),
                        InterviewerName = c.String(nullable: false, maxLength: 50),
                        FeedbackFile_Id = c.Int(nullable: false),
                        PositionCandidate_PositionID = c.Int(nullable: false),
                        PositionCandidate_CandidateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TechnicalInterviewId)
                .ForeignKey("dbo.FileBlobs", t => t.FeedbackFile_Id)
                .ForeignKey("dbo.PositionCandidates", t => new { t.PositionCandidate_PositionID, t.PositionCandidate_CandidateID })
                .Index(t => t.FeedbackFile_Id)
                .Index(t => new { t.PositionCandidate_PositionID, t.PositionCandidate_CandidateID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TechnicalInterviews", new[] { "PositionCandidate_PositionID", "PositionCandidate_CandidateID" }, "dbo.PositionCandidates");
            DropForeignKey("dbo.TechnicalInterviews", "FeedbackFile_Id", "dbo.FileBlobs");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Tags", "Position_PositionId", "dbo.Positions");
            DropForeignKey("dbo.PositionCandidates", "PositionID", "dbo.Positions");
            DropForeignKey("dbo.Positions", "PortfolioManager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Logs", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Logs", "Position_PositionId", "dbo.Positions");
            DropForeignKey("dbo.Positions", "LastOpenedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "LastClosedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "LastCancelledBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PositionCandidates", "CandidateID", "dbo.Candidates");
            DropForeignKey("dbo.FileBlobs", "Candidate_CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationSettings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TechnicalInterviews", new[] { "PositionCandidate_PositionID", "PositionCandidate_CandidateID" });
            DropIndex("dbo.TechnicalInterviews", new[] { "FeedbackFile_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Tags", new[] { "Position_PositionId" });
            DropIndex("dbo.Logs", new[] { "Position_PositionId" });
            DropIndex("dbo.Logs", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Positions", new[] { "LastClosedBy_Id" });
            DropIndex("dbo.Positions", new[] { "LastCancelledBy_Id" });
            DropIndex("dbo.Positions", new[] { "LastOpenedBy_Id" });
            DropIndex("dbo.Positions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Positions", new[] { "PortfolioManager_Id" });
            DropIndex("dbo.PositionCandidates", new[] { "CandidateID" });
            DropIndex("dbo.PositionCandidates", new[] { "PositionID" });
            DropIndex("dbo.FileBlobs", new[] { "Candidate_CandidateId" });
            DropIndex("dbo.Candidates", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ApplicationSettings", new[] { "ApplicationUser_Id" });
            DropTable("dbo.TechnicalInterviews");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.Logs");
            DropTable("dbo.Positions");
            DropTable("dbo.PositionCandidates");
            DropTable("dbo.FileBlobs");
            DropTable("dbo.Candidates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ApplicationSettings");
        }
    }
}
