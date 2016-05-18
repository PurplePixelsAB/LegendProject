using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace LegendWorld.Data.Items
{
    public abstract class WeaponItem : IItem
    {
        public WeaponItem()
        {
            this.Category = ItemCategory.Weapon;
        }

        public byte Power { get; set; }
        public int Speed { get; set; }
        public bool IsTwoHanded { get; set; }
        public ItemData Data { get; set; }
        public ItemCategory Category { get; protected set; }
        public int Weight { get; protected set; }
    }
}
