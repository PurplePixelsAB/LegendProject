using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace LegendWorld.Data.Items
{
    public abstract class WeaponItem : Item
    {
        public WeaponItem()
        {
            this.Category = ItemCategory.Weapon;
            this.Count = 1;
        }

        public byte Power { get; set; }
        public int Speed { get; set; }
        public bool IsTwoHanded { get; set; }
        //public ItemModel Data { get; set; }
        //public ItemCategory Category { get; protected set; }
        //public int Weight { get; protected set; }
        public int SwingRange { get; protected set; }
        public int SwingFov { get; protected set; }

        public override string ToString()
        {
            //if (this.Data.Count > 0)
            //    return string.Format("{1} {0}", this.Data.Identity, this.Data.Count);

            return string.Format("{0}", this.Identity);

        }

        public override int GetTotalWeight()
        {
            return this.Weight;
        }
    }
}
