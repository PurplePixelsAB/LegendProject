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
            this.Identity = ItemIdentity.Sword;

            this.Weight = 7000;
            this.Power = 18;
            this.Speed = 125;
            this.IsTwoHanded = false;
            this.SwingRange = 60;
            this.SwingFov = 120;
        }
    }
}
