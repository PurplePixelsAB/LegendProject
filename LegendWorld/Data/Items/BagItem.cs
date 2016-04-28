using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class BagItem : Item //Container
    {
        public List<Item> ItemsInBag { get; protected set; }
        public int GetTotaltWeight() { return this.ItemsInBag.Sum(i => i.Weight); }
        public int Count { get { return this.ItemsInBag.Count; } }

        public BagItem()
        {
            this.ItemsInBag = new List<Item>();
            this.Name = "Bag";
            this.StackCount = 1;
            this.IsStackable = false;
            this.Weight = 1;
            this.Category = ItemCategory.Container;
        }

        public void AddItem(Item itemToAdd)
        {
            this.ItemsInBag.Add(itemToAdd);
            //itemToAdd.Location.SetLocation(this);
        }
        public void RemoveItem(Item itemToRemove)
        {
            this.ItemsInBag.Add(itemToRemove);
        }
    }
}
