namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eleven : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prices", "Tarif01h", c => c.Double());
            AlterColumn("dbo.Prices", "Tarif12h", c => c.Double());
            AlterColumn("dbo.Prices", "Tarif23h", c => c.Double());
            AlterColumn("dbo.Prices", "Tarif34h", c => c.Double());
            AlterColumn("dbo.Prices", "Tarif4Plus", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prices", "Tarif4Plus", c => c.Double(nullable: false));
            AlterColumn("dbo.Prices", "Tarif34h", c => c.Double(nullable: false));
            AlterColumn("dbo.Prices", "Tarif23h", c => c.Double(nullable: false));
            AlterColumn("dbo.Prices", "Tarif12h", c => c.Double(nullable: false));
            AlterColumn("dbo.Prices", "Tarif01h", c => c.Double(nullable: false));
        }
    }
}
