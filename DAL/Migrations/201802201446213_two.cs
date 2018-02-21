namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parkings", "Logitude", c => c.Double(nullable: false));
            AddColumn("dbo.Parkings", "Latitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parkings", "Latitude");
            DropColumn("dbo.Parkings", "Logitude");
        }
    }
}
