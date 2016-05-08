using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public abstract class WeaponItem : Item
    {
        public WeaponItem()
        {
            this.Category = ItemCategory.Weapon;
        }

        public byte Power { get; set; }
        public int Speed { get; set; }
        public bool IsTwoHanded { get; set; } 
    }
}
