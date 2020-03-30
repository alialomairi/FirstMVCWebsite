namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DisplayName", c => c.String(maxLength: 10));
            AddColumn("dbo.Users", "Email", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "Phone", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Phone");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "DisplayName");
        }
    }
}
