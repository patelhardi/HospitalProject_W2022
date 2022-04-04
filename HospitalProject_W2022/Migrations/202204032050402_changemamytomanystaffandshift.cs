namespace HospitalProject_W2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changemamytomanystaffandshift : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shifts", "SID", "dbo.Staffs");
            DropIndex("dbo.Shifts", new[] { "SID" });
            CreateTable(
                "dbo.StaffShifts",
                c => new
                    {
                        Staff_SID = c.Int(nullable: false),
                        Shift_SHID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Staff_SID, t.Shift_SHID })
                .ForeignKey("dbo.Staffs", t => t.Staff_SID, cascadeDelete: true)
                .ForeignKey("dbo.Shifts", t => t.Shift_SHID, cascadeDelete: true)
                .Index(t => t.Staff_SID)
                .Index(t => t.Shift_SHID);
            
            DropColumn("dbo.Shifts", "SID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "SID", c => c.Int(nullable: false));
            DropForeignKey("dbo.StaffShifts", "Shift_SHID", "dbo.Shifts");
            DropForeignKey("dbo.StaffShifts", "Staff_SID", "dbo.Staffs");
            DropIndex("dbo.StaffShifts", new[] { "Shift_SHID" });
            DropIndex("dbo.StaffShifts", new[] { "Staff_SID" });
            DropTable("dbo.StaffShifts");
            CreateIndex("dbo.Shifts", "SID");
            AddForeignKey("dbo.Shifts", "SID", "dbo.Staffs", "SID", cascadeDelete: true);
        }
    }
}
