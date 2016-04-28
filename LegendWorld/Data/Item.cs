using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public class Item
    {
        public ushort Id { get; set; }
        public ItemIdentity Identity { get; protected set; }
        public string Name { get; set; }
        public ItemCategory Category { get; set; }
        //public ItemLocation Location { get; set; }
        public ushort Weight { get; set; }
        public bool IsStackable { get; set; }
        public ushort StackCount { get; set; }

        public virtual string GetInventoryString()
        {
            if (this.IsStackable)
            {
                return string.Format("{1} {0}", this.Name, this.StackCount);
            }
            else
                return this.Name;
        }
    }
}
