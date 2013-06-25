namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedSession1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "UserId");
        }
    }
}
