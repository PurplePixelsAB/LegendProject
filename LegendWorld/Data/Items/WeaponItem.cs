using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public abstract class WeaponItem : Item
    {
        public byte Damage { get; set; }
        public int Speed { get; set; }
        public bool IsTwoHanded { get; set; }
    }
}
