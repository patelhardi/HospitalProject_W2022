namespace HospitalProject_W2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedepartmentinstaff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Staffs", "DID", c => c.Int(nullable: false));
            CreateIndex("dbo.Staffs", "DID");
            AddForeignKey("dbo.Staffs", "DID", "dbo.Departments", "DID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "DID", "dbo.Departments");
            DropIndex("dbo.Staffs", new[] { "DID" });
            AlterColumn("dbo.Staffs", "DID", c => c.String());
        }
    }
}
