namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEnable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Enabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Enabled");
        }
    }
}
