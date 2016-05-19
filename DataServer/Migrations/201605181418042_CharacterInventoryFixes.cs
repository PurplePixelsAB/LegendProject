namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterInventoryFixes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CharacterDatas", "InventoryID", "dbo.ItemDatas");
            DropIndex("dbo.CharacterDatas", new[] { "InventoryID" });
            AlterColumn("dbo.CharacterDatas", "InventoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.CharacterDatas", "InventoryID");
            AddForeignKey("dbo.CharacterDatas", "InventoryID", "dbo.ItemDatas", "ItemDataID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CharacterDatas", "InventoryID", "dbo.ItemDatas");
            DropIndex("dbo.CharacterDatas", new[] { "InventoryID" });
            AlterColumn("dbo.CharacterDatas", "InventoryID", c => c.Int());
            CreateIndex("dbo.CharacterDatas", "InventoryID");
            AddForeignKey("dbo.CharacterDatas", "InventoryID", "dbo.ItemDatas", "ItemDataID");
        }
    }
}
