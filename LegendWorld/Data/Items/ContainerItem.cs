using System.Collections.Generic;

namespace LegendWorld.Data.Items
{
    public abstract class ContainerItem : Item
    {
        public ContainerItem()
        {
            this.Items = new List<ushort>();
            this.Category = ItemCategory.Container;
        }

        public List<ushort> Items { get; set; }
        //public ushort Id { get; set; }
        //public ItemIdentity Identity { get; set; }
        //public ItemCategory Category { get; set; }

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