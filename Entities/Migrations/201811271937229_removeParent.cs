namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeParent : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Messages", "ParentId");
            RenameColumn(table: "dbo.Messages", name: "Parent_MessageId", newName: "ParentId");
            RenameIndex(table: "dbo.Messages", name: "IX_Parent_MessageId", newName: "IX_ParentId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_ParentId", newName: "IX_Parent_MessageId");
            RenameColumn(table: "dbo.Messages", name: "ParentId", newName: "Parent_MessageId");
            AddColumn("dbo.Messages", "ParentId", c => c.Int());
        }
    }
}
