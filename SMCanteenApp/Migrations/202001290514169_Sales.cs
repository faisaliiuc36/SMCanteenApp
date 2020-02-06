namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SalesId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(),
                        Date = c.DateTime(nullable: false),
                        Discount = c.Double(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.SalesId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.SalesItemLists",
                c => new
                    {
                        SalesItemId = c.Int(nullable: false, identity: true),
                        SalesId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.SalesItemId)
                .ForeignKey("dbo.Sales", t => t.SalesId, cascadeDelete: true)
                .Index(t => t.SalesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesItemLists", "SalesId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.SalesItemLists", new[] { "SalesId" });
            DropIndex("dbo.Sales", new[] { "EmployeeId" });
            DropTable("dbo.SalesItemLists");
            DropTable("dbo.Sales");
        }
    }
}
