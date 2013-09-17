namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSessionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Type");
        }
    }
}
