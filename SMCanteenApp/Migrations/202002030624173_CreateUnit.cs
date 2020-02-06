namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUnit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        UnitId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UnitId);
            
            AddColumn("dbo.Employees", "UnitId", c => c.Int());
            AddColumn("dbo.Employees", "InActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Employees", "UnitId");
            AddForeignKey("dbo.Employees", "UnitId", "dbo.Units", "UnitId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "UnitId", "dbo.Units");
            DropIndex("dbo.Employees", new[] { "UnitId" });
            DropColumn("dbo.Employees", "InActive");
            DropColumn("dbo.Employees", "UnitId");
            DropTable("dbo.Units");
        }
    }
}
