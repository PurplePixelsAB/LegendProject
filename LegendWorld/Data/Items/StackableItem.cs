using Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LegendWorld.Data.Items
{
    public abstract class StackableItem : IItem
    {
        public StackableItem()
        {
            this.Category = ItemCategory.Stackable;
        }
        
        public int StackCount { get { return this.Data.Count; } set { this.Data.Count = value; } }
        
        public int StackWeight { get { return this.Weight * this.StackCount; } }
        public ItemData Data { get; set; }
        public ItemCategory Category { get; protected set; }
        public int Weight { get; protected set; }

        public override string ToString()
        {
            if (this.Data.Count > 0)
                return string.Format("{1} {0}", this.Data.Identity, this.Data.Count);

            return string.Format("{0}", this.Data.Identity);

        }

        public int GetTotalWeight()
        {
            return this.StackWeight;
        }
    }
}