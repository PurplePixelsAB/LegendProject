using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public abstract class WeaponItem : Item
    {
        public WeaponItem()
        {
            this.Category = ItemCategory.Weapon;
        }

        public byte Power { get; set; }
        public int Speed { get; set; }
        public bool IsTwoHanded { get; set; }
        //public ushort Id { get; set; }
        //public ItemIdentity Identity { get; set; }
        //public ItemCategory Category { get; set; }
    }
}
