using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class SwordItem : WeaponItem
    {
        public SwordItem()
        {
            this.Weight = 5000;
            this.Power = 90;
            this.Speed = 125;
            this.IsTwoHanded = false;
        }
    }
}
