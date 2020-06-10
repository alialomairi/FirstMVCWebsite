namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        NodeId = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 50),
                        RewritePath = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.NodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Nodes");
        }
    }
}
