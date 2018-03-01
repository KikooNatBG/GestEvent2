namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twelve : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OpenHours", "StartHour", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OpenHours", "EndHour", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Plages", "StartHour", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Plages", "EndHour", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Plages", "EndHour", c => c.Double(nullable: false));
            AlterColumn("dbo.Plages", "StartHour", c => c.Double(nullable: false));
            AlterColumn("dbo.OpenHours", "EndHour", c => c.Double(nullable: false));
            AlterColumn("dbo.OpenHours", "StartHour", c => c.Double(nullable: false));
        }
    }
}
