namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        TutorialId = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Subjects", t => t.ParentId)
                .ForeignKey("dbo.Materials", t => t.TutorialId, cascadeDelete: true)
                .Index(t => t.TutorialId)
                .Index(t => t.ParentId);
            
            AddColumn("dbo.Materials", "CategoryId", c => c.Int());
            CreateIndex("dbo.Materials", "CategoryId");
            AddForeignKey("dbo.Materials", "CategoryId", "dbo.Categories", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "TutorialId", "dbo.Materials");
            DropForeignKey("dbo.Subjects", "ParentId", "dbo.Subjects");
            DropForeignKey("dbo.Materials", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropIndex("dbo.Subjects", new[] { "ParentId" });
            DropIndex("dbo.Subjects", new[] { "TutorialId" });
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.Materials", new[] { "CategoryId" });
            DropColumn("dbo.Materials", "CategoryId");
            DropTable("dbo.Subjects");
            DropTable("dbo.Categories");
        }
    }
}
