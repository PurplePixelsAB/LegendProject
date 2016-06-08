using LegendClient.World.Items;
using LegendWorld.Data;
using LegendWorld.Data.Items;
using System.Collections.Generic;
using WindowsClient.World;
using Microsoft.Xna.Framework.Graphics;
using System;
using Data;

namespace LegendClient.Screens
{
    public class BagClientItem : BagItem, IClientItem
    {
        public BagClientItem() : base()
        { }
        public BagClientItem(List<Item> worldItems) : base()
        {
            this.Items = worldItems;
        }

        public Texture2D Texture { get; set; }

        //public void LoadItems(ClientWorldState world)
        //{
        //    foreach (int itemId in BagItem.Items)
        //    {
        //        ItemsInBag.Add(world.GetItem(itemId));
        //    }
        //}

        //public virtual string GetInventoryString()
        //{
        //    if (this.Data.IsStackable)
        //    {
        //        return string.Format("{1} {0}", this.Data.Name, this.StackCount);
        //    }
        //    else
        //        return this.Data.Name;
        //}
    }
}