using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public abstract class StackableItem : Item
    {
        public StackableItem()
        {
            this.Category = ItemCategory.Stackable;
            this.StackCount = 1;
        }

        [DataMember]
        public int StackCount { get; set; }

        [NotMapped]
        public int StackWeight { get { return this.Weight * this.StackCount; } }
    }
}