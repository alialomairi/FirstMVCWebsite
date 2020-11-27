namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Gender", c => c.Byte());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Gender");
        }
    }
}
