namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Session_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Address = c.String(),
                        Notes = c.String(),
                        IsConfirmed = c.Boolean(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        RegistrationClosedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sessions");
        }
    }
}
