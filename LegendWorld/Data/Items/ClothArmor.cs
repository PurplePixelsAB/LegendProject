using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class ClothArmorItem : ArmorItem
    {
        public ClothArmorItem()
        {
            this.Identity = ItemIdentity.ClothRobe;

            Armor = .05f;
            Weight = 700;
        }
    }
}
