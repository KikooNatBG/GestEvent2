namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seven : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Name", c => c.String());
            AddColumn("dbo.Events", "PlaceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "PlaceName");
            DropColumn("dbo.Events", "Name");
        }
    }
}
