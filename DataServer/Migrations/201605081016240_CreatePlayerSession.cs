namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePlayerSession : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Items", "Identity", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "StackCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "StackCount");
            DropColumn("dbo.Items", "Identity");
            DropTable("dbo.PlayerSessions");
        }
    }
}
