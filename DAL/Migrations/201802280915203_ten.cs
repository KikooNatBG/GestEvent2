namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ten : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsAlwaysOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OpenHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartHour = c.Double(nullable: false),
                        EndHour = c.Double(nullable: false),
                        DayNumber = c.Int(nullable: false),
                        Parking_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarif = c.Double(nullable: false),
                        Tarif01h = c.Double(nullable: false),
                        Tarif12h = c.Double(nullable: false),
                        Tarif23h = c.Double(nullable: false),
                        Tarif34h = c.Double(nullable: false),
                        Tarif4Plus = c.Double(nullable: false),
                        Plage_Id = c.Int(),
                        Parking_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plages", t => t.Plage_Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Plage_Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.Plages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartHour = c.Double(nullable: false),
                        EndHour = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "Parking_Id", "dbo.Parkings");
            DropForeignKey("dbo.Prices", "Plage_Id", "dbo.Plages");
            DropForeignKey("dbo.OpenHours", "Parking_Id", "dbo.Parkings");
            DropIndex("dbo.Prices", new[] { "Parking_Id" });
            DropIndex("dbo.Prices", new[] { "Plage_Id" });
            DropIndex("dbo.OpenHours", new[] { "Parking_Id" });
            DropTable("dbo.Plages");
            DropTable("dbo.Prices");
            DropTable("dbo.OpenHours");
            DropTable("dbo.Parkings");
        }
    }
}
