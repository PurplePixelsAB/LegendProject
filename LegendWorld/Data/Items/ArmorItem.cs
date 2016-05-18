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
        }
        public int Armor { get; set; }
        public ItemCategory Category { get; protected set; }
        public ItemData Data { get; set; }
        public int Weight { get; protected set; }
    }
}