namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Materials", "TrainerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Materials", "TrainerId");
            AddForeignKey("dbo.Materials", "TrainerId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Materials", "TrainerId", "dbo.Users");
            DropIndex("dbo.Materials", new[] { "TrainerId" });
            DropColumn("dbo.Materials", "TrainerId");
        }
    }
}
