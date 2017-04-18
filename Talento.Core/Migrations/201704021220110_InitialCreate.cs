namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Competencies = c.String(nullable: false, maxLength: 300),
                        Description = c.String(nullable: false, maxLength: 300),
                        CratedOn = c.DateTime(),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        IsTcsEmployee = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id)
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
                "dbo.Tags",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Position_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Positions", t => t.Position_Id)
                .Index(t => t.Position_Id);
            
            CreateTable(
                "dbo.FileBlobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Candidate_Id = c.Int(nullable: false),
                        FileName = c.String(nullable: false),
                        Blob = c.Binary(),
                    })
                .PrimaryKey(t => new { t.Id, t.Candidate_Id })
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .Index(t => t.Candidate_Id);
            
            CreateTable(
                "dbo.PositionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Position_Id = c.Int(nullable: false),
                        Action = c.Int(nullable: false),
                        PreviousStatus = c.Int(nullable: false),
                        ActualStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.Position_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Position_Id);
            
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
                "dbo.PositionCandidates",
                c => new
                    {
                        Position_Id = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Position_Id, t.Candidate_Id })
                .ForeignKey("dbo.Positions", t => t.Position_Id, cascadeDelete: true)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id, cascadeDelete: true)
                .Index(t => t.Position_Id)
                .Index(t => t.Candidate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PositionLogs", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PositionLogs", "Position_Id", "dbo.Positions");
            DropForeignKey("dbo.FileBlobs", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.Tags", "Position_Id", "dbo.Positions");
            DropForeignKey("dbo.Positions", "PortfolioManager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "LastOpenedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "LastClosedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Positions", "LastCancelledBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PositionCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.PositionCandidates", "Position_Id", "dbo.Positions");
            DropForeignKey("dbo.Candidates", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PositionCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.PositionCandidates", new[] { "Position_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PositionLogs", new[] { "Position_Id" });
            DropIndex("dbo.PositionLogs", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FileBlobs", new[] { "Candidate_Id" });
            DropIndex("dbo.Tags", new[] { "Position_Id" });
            DropIndex("dbo.Positions", new[] { "LastClosedBy_Id" });
            DropIndex("dbo.Positions", new[] { "LastCancelledBy_Id" });
            DropIndex("dbo.Positions", new[] { "LastOpenedBy_Id" });
            DropIndex("dbo.Positions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Positions", new[] { "PortfolioManager_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Candidates", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Candidates", new[] { "Email" });
            DropTable("dbo.PositionCandidates");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PositionLogs");
            DropTable("dbo.FileBlobs");
            DropTable("dbo.Tags");
            DropTable("dbo.Positions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Candidates");
        }
    }
}
