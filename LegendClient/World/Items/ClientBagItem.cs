using LegendWorld.Data;
using LegendWorld.Data.Items;
using System.Collections.Generic;

namespace LegendClient.Screens
{
    public class ClientBagItem : BagItem
    {
        public List<Item> ItemsInBag { get; set; }

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