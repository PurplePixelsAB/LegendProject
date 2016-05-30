using System;
using System.Runtime.Serialization;
using Data;

namespace LegendWorld.Data.Items
{
    public abstract class ArmorItem : IItem
    {
        public ArmorItem()
        {
            this.Category = ItemCategory.Armor;
            //this.Data.Count = 1;
        }
        public float Armor { get; set; }
        public ItemCategory Category { get; protected set; }
        public ItemData Data { get; set; }
        public int Weight { get; protected set; }

        public int GetTotalWeight()
        {
            return this.Weight;
        }
        public override string ToString()
        {
            //if (this.Data.Count > 0)
            //    return string.Format("{1} {0}", this.Data.Identity, this.Data.Count);

            return string.Format("{0}", this.Data.Identity);

        }
    }
}