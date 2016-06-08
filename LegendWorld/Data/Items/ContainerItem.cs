using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Data;
using System.Linq;

namespace LegendWorld.Data.Items
{
    public abstract class ContainerItem : Item
    {
        public ContainerItem()
        {
            this.Items = new List<Item>();
            this.Category = ItemCategory.Container;
            this.Count = 1;
        }

        //public ItemCategory Category { get; protected set; }
        //public ItemModel Data { get; set; }
        public List<Item> Items { get; set; }
        //public int Weight { get; protected set; }

        public override int GetTotalWeight()
        {
            return this.Items.Sum(i => i.GetTotalWeight()) + this.Weight;
        }

        //public List<int> Items { get; set; }

        //public void AddItem(ushort id)
        //{
        //    if (this.Items.Contains(id))
        //        return;

        //    this.Items.Add(id);
        //}
        //public void RemoveItem(ushort id)
        //{
        //    if (!this.Items.Contains(id))
        //        return;

        //    this.Items.Remove(id);
        //}
        //public void AddItem(Item itemToAdd)
        //{
        //    this.AddItem((ushort)itemToAdd.Id);
        //}
        //public void RemoveItem(Item itemToRemove)
        //{
        //    this.RemoveItem((ushort)itemToRemove.Id);
        //}
        public override string ToString()
        {
            //if (this.Count > 0)
            //    return string.Format("{1} {0}", this.Identity, this.Count);

            return string.Format("{0}", this.Identity);

        }
    }
}