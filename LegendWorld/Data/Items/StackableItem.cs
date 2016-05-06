using System;

namespace LegendWorld.Data.Items
{
    public class StackableItem : Item
    {
        public StackableItem()
        {
            this.Category = ItemCategory.Stackable;
        }

        //public ItemCategory Category { get; set; }

        //public ushort Id { get; set; }

        //public ItemIdentity Identity { get; set; }

        public uint StackCount { get; set; }
    }
}