namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemCode = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        ImageFile = c.String(),
                        Image = c.Binary(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ItemCode)
				;
			    Sql("DBCC CHECKIDENT ('Items', RESEED, 1000)");
		}
        
        public override void Down()
        {
            DropTable("dbo.Items");
        }
    }
}
