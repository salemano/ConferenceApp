namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEventLogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ObjectId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        LogLevel = c.Int(nullable: false),
                        LogType = c.Int(nullable: false),
                        IPAddress = c.String(),
                        Message = c.String(),
                        Detail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventLogs");
        }
    }
}
