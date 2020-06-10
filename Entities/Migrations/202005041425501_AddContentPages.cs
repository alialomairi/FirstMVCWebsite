namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentPages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentPages",
                c => new
                    {
                        PageId = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        PageContent = c.String(),
                    })
                .PrimaryKey(t => t.PageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContentPages");
        }
    }
}
