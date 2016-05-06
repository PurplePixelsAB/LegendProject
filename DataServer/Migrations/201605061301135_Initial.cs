namespace DataServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Ability", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Ability");
        }
    }
}
