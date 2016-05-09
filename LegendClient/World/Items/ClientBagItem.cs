using LegendWorld.Data;
using LegendWorld.Data.Items;
using System.Collections.Generic;
using WindowsClient.World;

namespace LegendClient.Screens
{
    public class ClientBagItem
    {
        public ClientBagItem(BagItem bagItem)
        {
            this.BagItem = bagItem;
            this.ItemsInBag = new List<Item>(bagItem.Items.Count+1);
        }

        public BagItem BagItem { get; private set; }
        public List<Item> ItemsInBag { get; private set; }

        public void LoadItems(ClientWorldState world)
        {
            foreach (int itemId in BagItem.Items)
            {
                ItemsInBag.Add(world.GetItem(itemId));
            }
        }

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