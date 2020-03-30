namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFacebookId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FacebookId", c => c.String(maxLength: 50));
            AlterColumn("dbo.Users", "DisplayName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "DisplayName", c => c.String(maxLength: 10));
            DropColumn("dbo.Users", "FacebookId");
        }
    }
}
