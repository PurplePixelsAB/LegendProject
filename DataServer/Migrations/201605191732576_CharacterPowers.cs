namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterPowers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterPowerLearneds",
                c => new
                    {
                        CharacterPowerLearnedID = c.Int(nullable: false, identity: true),
                        Power = c.Int(nullable: false),
                        CharacterData_CharacterDataID = c.Int(),
                    })
                .PrimaryKey(t => t.CharacterPowerLearnedID)
                .ForeignKey("dbo.CharacterDatas", t => t.CharacterData_CharacterDataID)
                .Index(t => t.CharacterData_CharacterDataID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CharacterPowerLearneds", "CharacterData_CharacterDataID", "dbo.CharacterDatas");
            DropIndex("dbo.CharacterPowerLearneds", new[] { "CharacterData_CharacterDataID" });
            DropTable("dbo.CharacterPowerLearneds");
        }
    }
}
