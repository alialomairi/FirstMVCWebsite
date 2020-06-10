namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "CategoryKey");
        }
    }
}
