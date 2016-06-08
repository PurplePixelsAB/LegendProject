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
            this.Identity = ItemIdentity.PlateArmor;

            Armor = .3f;
            Weight = 27000;
        }
    }
}
