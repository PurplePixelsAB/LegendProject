namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroundItemFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroundItems", "CurrentMapId", c => c.Int(nullable: false));
            AddColumn("dbo.GroundItems", "PositionX", c => c.Int(nullable: false));
            AddColumn("dbo.GroundItems", "PositionY", c => c.Int(nullable: false));
            AddColumn("dbo.GroundItems", "ItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroundItems", "ItemId");
            DropColumn("dbo.GroundItems", "PositionY");
            DropColumn("dbo.GroundItems", "PositionX");
            DropColumn("dbo.GroundItems", "CurrentMapId");
        }
    }
}
