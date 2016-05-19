namespace DataServer.Migrations
{
    using Data;
    using LegendWorld.Data;
    using LegendWorld.Data.Abilities;
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
            context.Database.ExecuteSqlCommand("DELETE FROM ItemDatas");
            context.Database.ExecuteSqlCommand("DELETE FROM PlayerSessions");
            context.Database.ExecuteSqlCommand("DELETE FROM CharacterDatas");

            Random rnd = new Random();
            foreach (CharacterPowerIdentity abilityId in Enum.GetValues(typeof(CharacterPowerIdentity)))
            {
                if (abilityId == CharacterPowerIdentity.DefaultAttack)
                    continue;

                for (ushort i = 0; i < 10; i++)
                {
                    ItemData item = context.Items.Create(); // new ItemData();
                    //item.Id = itemId;
                    item.Identity = ItemData.ItemIdentity.PowerScoll;
                    item.SubType = (int)abilityId;
                    item.MoveTo(0, new Point(rnd.Next(1, 5000), rnd.Next(1, 5000)));
                    item.Count = rnd.Next(1, 9);
                    var itemAdded = context.Items.Add(item);
                    //itemId++;
                }
            }
            context.SaveChanges();

            foreach (ItemData.ItemIdentity itemID in Enum.GetValues(typeof(ItemData.ItemIdentity)))
            {
                if (itemID == ItemData.ItemIdentity.PowerScoll || itemID == ItemData.ItemIdentity.Corpse)
                    continue;

                for (ushort i = 0; i < 10; i++)
                {
                    ItemData item = context.Items.Create();
                    item.Identity = itemID; // ItemData.ItemIdentity.PowerScoll;
                    //item.SubType = (int)abilityId;
                    item.MoveTo(0, new Point(rnd.Next(1, 5000), rnd.Next(1, 5000)));
                    item.Count = rnd.Next(1, 9);
                    var itemAdded = context.Items.Add(item);
                    //itemId++;
                }

                context.SaveChanges();
            }

            for (int i = 1; i < 10; i++)
            {
                CharacterData character = context.Characters.Create();
                ItemData charInventory = context.Items.Create();
                charInventory.Identity = ItemData.ItemIdentity.Bag;
                var inventoryAdded = context.Items.Add(charInventory);
                character.Name = "TempCharacter" + i;
                character.Inventory = inventoryAdded;
                context.Characters.Add(character);
            }

            context.SaveChanges();

            //var itemList = context.Items.ToList();
            //foreach (ItemData item in itemList)
            //{
            //    GroundItem groundItem = new GroundItem();
            //    groundItem.ItemId = item.Id;
            //    groundItem.Position = new Point(rnd.Next(1, 5000), rnd.Next(1, 5000));
            //    context.GroundItems.AddOrUpdate(groundItem);
            //}
        }
    }
}
