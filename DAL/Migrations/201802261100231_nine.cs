namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventImages", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventImages", new[] { "Event_Id" });
            DropTable("dbo.EventImages");
        }
    }
}
