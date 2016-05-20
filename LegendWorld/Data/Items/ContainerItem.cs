using System.Collections.Generic;
using System.Runtime.Serialization;
using Data;

namespace LegendWorld.Data.Items
{
    public abstract class ContainerItem : IItem
    {
        public ContainerItem()
        {
            this.Items = new List<IItem>();
            this.Category = ItemCategory.Container;
        }

        public ItemCategory Category { get; protected set; }
        public ItemData Data { get; set; }
        public List<IItem> Items { get; set; }
        public int Weight { get; protected set; }

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
            if (this.Data.Count > 0)
                return string.Format("{1} {0}", this.Data.Identity, this.Data.Count);

            return string.Format("{0}", this.Data.Identity);

        }
    }
}