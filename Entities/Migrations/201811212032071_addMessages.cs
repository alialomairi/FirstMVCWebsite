namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        MessageType = c.Int(nullable: false),
                        MessageSubject = c.String(),
                        MessageText = c.String(),
                        ParentId = c.Int(),
                        Parent_MessageId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Messages", t => t.Parent_MessageId)
                .Index(t => t.Parent_MessageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Parent_MessageId", "dbo.Messages");
            DropIndex("dbo.Messages", new[] { "Parent_MessageId" });
            DropTable("dbo.Messages");
        }
    }
}
