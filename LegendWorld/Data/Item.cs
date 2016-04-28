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
        public ItemIdentity Identity { get; set; }
        public ItemCategory Category { get; set; }

        //public virtual string GetInventoryString()
        //{
        //    if (this.Data.IsStackable)
        //    {
        //        return string.Format("{1} {0}", this.Data.Name, this.StackCount);
        //    }
        //    else
        //        return this.Data.Name;
        //}

        //public void OnUse()
        //{

        //}
    }
}
