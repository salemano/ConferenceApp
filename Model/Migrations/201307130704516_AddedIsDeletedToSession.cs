namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsDeletedToSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "IsDeleted", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "IsDeleted");
        }
    }
}
