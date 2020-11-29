namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Materials", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Materials", "CreationDate");
        }
    }
}
