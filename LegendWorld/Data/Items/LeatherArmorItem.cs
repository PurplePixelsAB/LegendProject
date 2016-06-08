using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class LeatherArmorItem : ArmorItem
    {
        public LeatherArmorItem()
        {
            this.Identity = ItemIdentity.LeatherArmor;

            Armor = .2f;
            Weight = 5000;
        }
    }
}
