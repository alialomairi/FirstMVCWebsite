namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "SubjectKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "SubjectKey");
        }
    }
}
