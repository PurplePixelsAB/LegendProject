using Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LegendWorld.Data.Items
{
    public abstract class StackableItem : Item
    {
        public StackableItem()
        {
            this.Category = ItemCategory.Stackable;
        }
        
        //public int Count { get; set; }
        
        public int StackWeight { get { return this.Weight * this.Count; } }
        //public ItemModel Data { get; set; }
        //public ItemCategory Category { get; protected set; }
        //public int Weight { get; protected set; }

        public override string ToString()
        {
            if (this.Count > 0)
                return string.Format("{1} {0}", this.Identity, this.Count);

            return string.Format("{0}", this.Identity);

        }

        public override int GetTotalWeight()
        {
            return this.StackWeight;
        }
    }
}