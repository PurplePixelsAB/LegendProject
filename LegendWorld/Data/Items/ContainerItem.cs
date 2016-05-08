using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public abstract class ContainerItem : Item
    {
        public ContainerItem()
        {
            this.Items = new List<int>();
            this.Category = ItemCategory.Container;
        }

        [DataMember]
        public List<int> Items { get; set; }

        public void AddItem(ushort id)
        {
            if (this.Items.Contains(id))
                return;

            this.Items.Add(id);
        }
        public void RemoveItem(ushort id)
        {
            if (!this.Items.Contains(id))
                return;

            this.Items.Remove(id);
        }
        public void AddItem(Item itemToAdd)
        {
            this.AddItem((ushort)itemToAdd.Id);
        }
        public void RemoveItem(Item itemToRemove)
        {
            this.RemoveItem((ushort)itemToRemove.Id);
        }
    }
}