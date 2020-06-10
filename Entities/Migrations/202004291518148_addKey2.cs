namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKey2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Materials", "MaterialKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Materials", "MaterialKey");
        }
    }
}
