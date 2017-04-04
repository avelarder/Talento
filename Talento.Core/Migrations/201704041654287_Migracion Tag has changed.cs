namespace Talento.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionTaghaschanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Position_Id", "dbo.Positions");
            DropIndex("dbo.Tags", new[] { "Position_Id" });
            AddColumn("dbo.Tags", "Position_Id1", c => c.Int());
            AlterColumn("dbo.Tags", "Position_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Tags", "Position_Id1");
            AddForeignKey("dbo.Tags", "Position_Id1", "dbo.Positions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Position_Id1", "dbo.Positions");
            DropIndex("dbo.Tags", new[] { "Position_Id1" });
            AlterColumn("dbo.Tags", "Position_Id", c => c.Int());
            DropColumn("dbo.Tags", "Position_Id1");
            CreateIndex("dbo.Tags", "Position_Id");
            AddForeignKey("dbo.Tags", "Position_Id", "dbo.Positions", "Id");
        }
    }
}
