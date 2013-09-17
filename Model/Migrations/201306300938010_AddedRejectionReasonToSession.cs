namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRejectionReasonToSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "RejectionReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "RejectionReason");
        }
    }
}
