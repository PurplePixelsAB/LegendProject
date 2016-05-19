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
                        CharacterDataID = c.Int(nullable: false, identity: true),
                        MapID = c.Int(nullable: false),
                        WorldX = c.Int(nullable: false),
                        WorldY = c.Int(nullable: false),
                        Name = c.String(),
                        Health = c.Byte(nullable: false),
                        Energy = c.Byte(nullable: false),
                        InventoryID = c.Int(),
                        HolsterID = c.Int(),
                        ArmorID = c.Int(),
                        LeftHandID = c.Int(),
                        RightHandID = c.Int(),
                    })
                .PrimaryKey(t => t.CharacterDataID)
                .ForeignKey("dbo.ItemDatas", t => t.ArmorID)
                .ForeignKey("dbo.ItemDatas", t => t.HolsterID)
                .ForeignKey("dbo.ItemDatas", t => t.InventoryID)
                .ForeignKey("dbo.ItemDatas", t => t.LeftHandID)
                .ForeignKey("dbo.ItemDatas", t => t.RightHandID)
                .Index(t => t.InventoryID)
                .Index(t => t.HolsterID)
                .Index(t => t.ArmorID)
                .Index(t => t.LeftHandID)
                .Index(t => t.RightHandID);
            
            CreateTable(
                "dbo.ItemDatas",
                c => new
                    {
                        ItemDataID = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        SubType = c.Int(),
                        Count = c.Int(nullable: false),
                        WorldMapID = c.Int(),
                        WorldX = c.Int(),
                        WorldY = c.Int(),
                        ContainerID = c.Int(),
                    })
                .PrimaryKey(t => t.ItemDataID)
                .ForeignKey("dbo.ItemDatas", t => t.ContainerID)
                .Index(t => t.ContainerID);
            
            CreateTable(
                "dbo.PlayerSessions",
                c => new
                    {
                        PlayerSessionID = c.Int(nullable: false, identity: true),
                        ClientAddress = c.String(),
                        Created = c.DateTime(nullable: false),
                        CharacterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerSessionID)
                .ForeignKey("dbo.CharacterDatas", t => t.CharacterID, cascadeDelete: true)
                .Index(t => t.CharacterID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerSessions", "CharacterID", "dbo.CharacterDatas");
            DropForeignKey("dbo.CharacterDatas", "RightHandID", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "LeftHandID", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "InventoryID", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "HolsterID", "dbo.ItemDatas");
            DropForeignKey("dbo.CharacterDatas", "ArmorID", "dbo.ItemDatas");
            DropForeignKey("dbo.ItemDatas", "ContainerID", "dbo.ItemDatas");
            DropIndex("dbo.PlayerSessions", new[] { "CharacterID" });
            DropIndex("dbo.ItemDatas", new[] { "ContainerID" });
            DropIndex("dbo.CharacterDatas", new[] { "RightHandID" });
            DropIndex("dbo.CharacterDatas", new[] { "LeftHandID" });
            DropIndex("dbo.CharacterDatas", new[] { "ArmorID" });
            DropIndex("dbo.CharacterDatas", new[] { "HolsterID" });
            DropIndex("dbo.CharacterDatas", new[] { "InventoryID" });
            DropTable("dbo.PlayerSessions");
            DropTable("dbo.ItemDatas");
            DropTable("dbo.CharacterDatas");
        }
    }
}
