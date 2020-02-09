namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteEmployeeBadgeNo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "BadgeNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "BadgeNumber", c => c.String());
        }
    }
}
