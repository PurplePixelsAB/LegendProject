using System;

namespace LegendWorld.Data.Items
{
    public abstract class ArmorItem : Item
    {
        public ArmorItem()
        {
            this.Category = ItemCategory.Armor;
        }
        public int Armor { get; set; }

        //public ItemCategory Category { get; set; }

        //public ushort Id { get; set; }

        //public ItemIdentity Identity { get; set; }
    }
}