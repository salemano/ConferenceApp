namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_FileData_Image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        UserId = c.Int(nullable: false),
                        FileSize = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        FileDataId = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDatas", t => t.FileDataId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.FileDataId)
                .Index(t => t.UserId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.FileDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "PhotoId", c => c.Int());
            AddForeignKey("dbo.Users", "PhotoId", "dbo.Images", "Id");
            CreateIndex("dbo.Users", "PhotoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Images", new[] { "User_Id" });
            DropIndex("dbo.Images", new[] { "UserId" });
            DropIndex("dbo.Images", new[] { "FileDataId" });
            DropIndex("dbo.Users", new[] { "PhotoId" });
            DropForeignKey("dbo.Images", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            DropForeignKey("dbo.Images", "FileDataId", "dbo.FileDatas");
            DropForeignKey("dbo.Users", "PhotoId", "dbo.Images");
            DropColumn("dbo.Users", "PhotoId");
            DropTable("dbo.FileDatas");
            DropTable("dbo.Images");
        }
    }
}
