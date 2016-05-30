using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class PlateArmorItem : ArmorItem
    {
        public PlateArmorItem()
        {
            Armor = .3f;
            Weight = 27000;
        }
    }
}
