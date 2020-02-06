namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSalesItemList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesItemLists", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.SalesItemLists", "ItemId");
            AddForeignKey("dbo.SalesItemLists", "ItemId", "dbo.Items", "ItemCode", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesItemLists", "ItemId", "dbo.Items");
            DropIndex("dbo.SalesItemLists", new[] { "ItemId" });
            DropColumn("dbo.SalesItemLists", "ItemId");
        }
    }
}
