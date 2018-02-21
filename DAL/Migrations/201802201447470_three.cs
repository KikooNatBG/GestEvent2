namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class three : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parkings", "Longitude", c => c.Double(nullable: false));
            DropColumn("dbo.Parkings", "Logitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parkings", "Logitude", c => c.Double(nullable: false));
            DropColumn("dbo.Parkings", "Longitude");
        }
    }
}
