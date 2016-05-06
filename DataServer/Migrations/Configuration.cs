namespace DataServer.Migrations
{
    using LegendWorld.Data;
    using LegendWorld.Data.Abilities;
    using LegendWorld.Data.Items;
    using Microsoft.Xna.Framework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataServer.Models.WorldDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DataServer.Models.WorldDbContext";
        }

        protected override void Seed(DataServer.Models.WorldDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            //ushort itemId = 1;
            foreach (AbilityIdentity abilityId in Enum.GetValues(typeof(AbilityIdentity)))
            {
                Random rnd = new Random();
                for (ushort i = 0; i < 10; i++)
                {
                    AbilityScrollItem item = new AbilityScrollItem();
                    //item.Id = itemId;
                    item.Ability = abilityId;
                    //itemId++;
                    context.Items.AddOrUpdate(item);

                    GroundItem groundItem = new GroundItem();
                    groundItem.ItemId = (ushort)item.Id;
                    //groundItem.Id = itemId;
                    groundItem.Position = new Point(rnd.Next(1, 1000), rnd.Next(1, 1000));
                    context.GroundItems.AddOrUpdate(groundItem);

                    //itemId++;
                }
            }
        }
    }
}
