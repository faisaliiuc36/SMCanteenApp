namespace SMCanteenApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmployeeId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Emp_Id", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Emp_Id", c => c.Int(nullable: false));
        }
    }
}
