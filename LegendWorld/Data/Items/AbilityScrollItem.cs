using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class AbilityScrollItem : Item //Consumable
    {
        public AbilityIdentity Ability { get; set; }
        public AbilityScrollItem()
        {
            this.Name = "Ability Scroll";
        }
        public void OnUse(Character usedBy)
        {
            if (usedBy.Teach(this.Ability))
            {

            }
        }
    }
}
