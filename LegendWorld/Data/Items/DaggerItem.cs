using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class DaggerItem : WeaponItem
    {
        public DaggerItem()
        {
            this.Weight = 1000;
            this.Power = 8;
            this.Speed = 175;
            this.IsTwoHanded = false;
            this.SwingRange = 40;
            this.SwingFov = 90;
        }
    }
}
