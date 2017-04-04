namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrationwithoriginalmodel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tags", new[] { "Position_Id1" });
            DropColumn("dbo.Tags", "Position_Id");
            RenameColumn(table: "dbo.Tags", name: "Position_Id1", newName: "Position_Id");
            AlterColumn("dbo.Tags", "Position_Id", c => c.Int());
            CreateIndex("dbo.Tags", "Position_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tags", new[] { "Position_Id" });
            AlterColumn("dbo.Tags", "Position_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Tags", name: "Position_Id", newName: "Position_Id1");
            AddColumn("dbo.Tags", "Position_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Tags", "Position_Id1");
        }
    }
}