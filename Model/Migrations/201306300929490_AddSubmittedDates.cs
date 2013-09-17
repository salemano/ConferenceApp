namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmittedDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "UserSubmittedAt", c => c.DateTime());
            AddColumn("dbo.Sessions", "AdminSubmittedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "AdminSubmittedAt");
            DropColumn("dbo.Sessions", "UserSubmittedAt");
        }
    }
}
