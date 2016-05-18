namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
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
                        Id = c.Int(nullable: false, identity: true),
                        CharacterId = c.Int(nullable: false),
                        ClientAddress = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemDatas", "ContainerID", "dbo.ItemDatas");
            DropIndex("dbo.ItemDatas", new[] { "ContainerID" });
            DropTable("dbo.PlayerSessions");
            DropTable("dbo.ItemDatas");
        }
    }
}
