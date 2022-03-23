namespace HospitalProject_W2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createshiftentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        SHID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.SHID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Shifts");
        }
    }
}
