namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Comment = c.String(),
                        ActivationToken = c.Guid(),
                        ActivatedAt = c.DateTime(),
                        RegisteredAt = c.DateTime(),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        PasswordRecoveryToken = c.Guid(),
                        IsAdministrator = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
