namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Comment");
            DropColumn("dbo.Users", "PhoneNumber");
            DropColumn("dbo.Users", "DateOfBirth");
        }
    }
}
