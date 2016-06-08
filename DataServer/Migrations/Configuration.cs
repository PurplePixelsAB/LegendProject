namespace DataServer.Migrations
{
    using Data;
    //using LegendWorld.Data;
    using Microsoft.Xna.Framework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataServer.Models.WorldDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
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
            context.Database.ExecuteSqlCommand("DELETE FROM CharacterPowerLearneds");
            context.Database.ExecuteSqlCommand("DELETE FROM ItemDatas");
            context.Database.ExecuteSqlCommand("DELETE FROM PlayerSessions");
            context.Database.ExecuteSqlCommand("DELETE FROM CharacterDatas");

            Random rnd = new Random();
            foreach (LegendWorld.Data.CharacterPowerIdentity abilityId in Enum.GetValues(typeof(LegendWorld.Data.CharacterPowerIdentity)))
            {
                if (abilityId == LegendWorld.Data.CharacterPowerIdentity.DefaultAttack)
                    continue;

                for (ushort i = 0; i < 10; i++)
                {
                    Item item = context.Items.Create(); // new ItemData();
                    //item.Id = itemId;
                    item.Identity = (int)LegendWorld.Data.ItemIdentity.PowerScoll;
                    item.SubType = (int)abilityId;
                    //item.MoveTo(0, new Point(rnd.Next(1, 5000), rnd.Next(1, 5000)));
                    item.WorldMapId = 0;
                    item.WorldX = rnd.Next(1, 5000);
                    item.WorldY = rnd.Next(1, 5000);
                    item.Count = rnd.Next(1, 9);
                    var itemAdded = context.Items.Add(item);
                    //itemId++;
                }
            }
            context.SaveChanges();

            foreach (LegendWorld.Data.ItemIdentity itemID in Enum.GetValues(typeof(LegendWorld.Data.ItemIdentity)))
            {
                if (itemID == LegendWorld.Data.ItemIdentity.PowerScoll || itemID == LegendWorld.Data.ItemIdentity.Corpse)
                    continue;

                for (ushort i = 0; i < 10; i++)
                {
                    Item item = context.Items.Create();
                    item.Identity = (int)itemID; // ItemIdentity.PowerScoll;
                    //item.SubType = (int)abilityId;
                    //item.MoveTo(0, new Point(, rnd.Next(1, 5000)));
                    item.WorldMapId = 0;
                    item.WorldX = rnd.Next(1, 5000);
                    item.WorldY = rnd.Next(1, 5000);
                    item.Count = 1;

                    if (itemID == LegendWorld.Data.ItemIdentity.Gold)
                        item.Count = rnd.Next(1, 9999);
                    else if (itemID == LegendWorld.Data.ItemIdentity.Bandage)
                        item.Count = rnd.Next(5, 99);

                    var itemAdded = context.Items.Add(item);
                    //itemId++;
                }

                context.SaveChanges();
            }

            for (int i = 1; i < 10; i++)
            {
                Character character = context.Characters.Create();
                Item charInventory = context.Items.Create();
                charInventory.Identity = (int)LegendWorld.Data.ItemIdentity.Bag;
                var inventoryAdded = context.Items.Add(charInventory);
                character.Name = "TempCharacter" + i;
                character.InventoryId = inventoryAdded.Id;
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
