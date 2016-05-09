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
            context.Database.ExecuteSqlCommand("DELETE FROM Items");
            context.Database.ExecuteSqlCommand("DELETE FROM GroundItems");

            Random rnd = new Random();
            foreach (AbilityIdentity abilityId in Enum.GetValues(typeof(AbilityIdentity)))
            {
                if (abilityId == AbilityIdentity.DefaultAttack)
                    continue;

                for (ushort i = 0; i < 10; i++)
                {
                    AbilityScrollItem item = new AbilityScrollItem();
                    //item.Id = itemId;
                    item.Ability = abilityId;
                    //itemId++;
                    var itemAdded = context.Items.Add(item);
                    //itemId++;
                }
            }
            context.SaveChanges();

            var itemList = context.Items.ToList();
            foreach (Item item in itemList)
            {
                GroundItem groundItem = new GroundItem();
                groundItem.ItemId = item.Id;
                groundItem.Position = new Point(rnd.Next(1, 5000), rnd.Next(1, 5000));
                context.GroundItems.AddOrUpdate(groundItem);
            }
        }
    }
}
