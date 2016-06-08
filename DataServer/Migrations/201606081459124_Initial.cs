namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterPowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharacterId = c.Int(nullable: false),
                        Power = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MapId = c.Int(nullable: false),
                        WorldX = c.Int(nullable: false),
                        WorldY = c.Int(nullable: false),
                        Name = c.String(),
                        Health = c.Int(nullable: false),
                        Energy = c.Int(nullable: false),
                        InventoryId = c.Int(nullable: false),
                        ArmorId = c.Int(),
                        LeftHandId = c.Int(),
                        RightHandId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        SubType = c.Int(),
                        Count = c.Int(nullable: false),
                        WorldMapId = c.Int(),
                        WorldX = c.Int(),
                        WorldY = c.Int(),
                        ContainerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharacterId = c.Int(nullable: false),
                        ClientAddress = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlayerSessions");
            DropTable("dbo.Items");
            DropTable("dbo.Characters");
            DropTable("dbo.CharacterPowers");
        }
    }
}
