namespace HospitalProject_W2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createrelationshipstaffandshift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "SID", c => c.Int(nullable: false));
            CreateIndex("dbo.Shifts", "SID");
            AddForeignKey("dbo.Shifts", "SID", "dbo.Staffs", "SID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "SID", "dbo.Staffs");
            DropIndex("dbo.Shifts", new[] { "SID" });
            DropColumn("dbo.Shifts", "SID");
        }
    }
}
