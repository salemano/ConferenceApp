namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageNullableuploadedByUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            DropIndex("dbo.Images", new[] { "UserId" });
            AlterColumn("dbo.Images", "UserId", c => c.Int());
            AddForeignKey("dbo.Images", "UserId", "dbo.Users", "Id");
            CreateIndex("dbo.Images", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Images", new[] { "UserId" });
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            AlterColumn("dbo.Images", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Images", "UserId");
            AddForeignKey("dbo.Images", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
    