namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Title", c => c.String());
            AddColumn("dbo.Sessions", "Overview", c => c.String());
            AddColumn("dbo.Sessions", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "IsAccepted", c => c.Boolean());
            AddColumn("dbo.Sessions", "City", c => c.String());
            DropColumn("dbo.Sessions", "Subject");
            DropColumn("dbo.Sessions", "Address");
            DropColumn("dbo.Sessions", "Notes");
            DropColumn("dbo.Sessions", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sessions", "IsConfirmed", c => c.Boolean());
            AddColumn("dbo.Sessions", "Notes", c => c.String());
            AddColumn("dbo.Sessions", "Address", c => c.String());
            AddColumn("dbo.Sessions", "Subject", c => c.String());
            DropColumn("dbo.Sessions", "City");
            DropColumn("dbo.Sessions", "IsAccepted");
            DropColumn("dbo.Sessions", "CreatedAt");
            DropColumn("dbo.Sessions", "Overview");
            DropColumn("dbo.Sessions", "Title");
        }
    }
}
