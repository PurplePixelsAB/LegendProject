namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MapId = c.Int(nullable: false),
                        WorldX = c.Int(nullable: false),
                        WorldY = c.Int(nullable: false),
                        Name = c.String(),
                        Health = c.Byte(nullable: false),
                        Energy = c.Byte(nullable: false),
                        InventoryId = c.Int(nullable: false),
                        ArmorId = c.Int(),
                        LeftHandId = c.Int(),
                        RightHandId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemDatas", t => t.ArmorId)
                .ForeignKey("dbo.ItemDatas", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.ItemDatas", t => t.LeftHandId)
                .ForeignKey("dbo.ItemDatas", t => t.RightHandId)
                .Index(t => t.InventoryId)
                .Index(t => t.ArmorId)
                .Index(t => t.LeftHandId)
                .Index(t => t.RightHandId);
            
            CreateTable(
                "dbo.ItemDatas",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemDatas", t => t.ContainerId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.CharacterPowerLearneds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharacterID = c.Int(nullable: false),
                        Power = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CharacterDatas", t => t.CharacterID, cascadeDelete: true)
                .Index(t => t.CharacterID);
            
            CreateTable(
                "dbo.PlayerSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientAddress = c.String(),
                        Created = c.DateTime(nullable: false),
                        CharacterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CharacterDatas", t => t.CharacterID, cascadeDelete: true)
                .Index(t => t.CharacterID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerSessions", "CharacterID", "dbo.CharacterDatas");
            DropForeignKey("dbo.CharacterDatas", "RightHandId", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterPowerLearneds", "CharacterID", "dbo.CharacterDatas");
            DropForeignKey("dbo.CharacterDatas", "LeftHandId", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "InventoryId", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "ArmorId", "dbo.ItemDatas");
            DropForeignKey("dbo.ItemDatas", "ContainerId", "dbo.ItemDatas");
            DropIndex("dbo.PlayerSessions", new[] { "CharacterID" });
            DropIndex("dbo.CharacterPowerLearneds", new[] { "CharacterID" });
            DropIndex("dbo.ItemDatas", new[] { "ContainerId" });
            DropIndex("dbo.CharacterDatas", new[] { "RightHandId" });
            DropIndex("dbo.CharacterDatas", new[] { "LeftHandId" });
            DropIndex("dbo.CharacterDatas", new[] { "ArmorId" });
            DropIndex("dbo.CharacterDatas", new[] { "InventoryId" });
            DropTable("dbo.PlayerSessions");
            DropTable("dbo.CharacterPowerLearneds");
            DropTable("dbo.ItemDatas");
            DropTable("dbo.CharacterDatas");
        }
    }
}
