namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Materials", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Materials", "Description");
        }
    }
}
