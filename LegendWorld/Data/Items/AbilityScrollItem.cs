using Data.World;
using LegendWorld.Data.Abilities;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class AbilityScrollItem : ConsumableItem //Consumable
    {
        public AbilityIdentity Ability { get { return (AbilityIdentity)base.StackCount; } set { base.StackCount = (uint)value; } }
        public AbilityScrollItem()
        {
            //this.Name = "Ability Scroll";
            //this.IsStackable = false;
            this.Identity = ItemIdentity.AbilityScoll;
            this.Category = ItemCategory.Consumable;
            //this.Weight = 100;
        }
        public override bool OnUse(Character usedBy)
        {
            if (usedBy.Teach(this.Ability))
            {
                return true;
            }

            return false;
        }
    }
}
