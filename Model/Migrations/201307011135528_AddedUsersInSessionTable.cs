namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsersInSessionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersInSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.SessionId)
                .Index(t => t.UserId);
            
            AddForeignKey("dbo.Sessions", "UserId", "dbo.Users", "Id", cascadeDelete: false);
            CreateIndex("dbo.Sessions", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropIndex("dbo.UsersInSessions", new[] { "UserId" });
            DropIndex("dbo.UsersInSessions", new[] { "SessionId" });
            DropForeignKey("dbo.Sessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersInSessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersInSessions", "SessionId", "dbo.Sessions");
            DropTable("dbo.UsersInSessions");
        }
    }
}
