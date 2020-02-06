namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "InActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Items", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Image", c => c.Binary(nullable: false));
            DropColumn("dbo.Items", "InActive");
        }
    }
}
