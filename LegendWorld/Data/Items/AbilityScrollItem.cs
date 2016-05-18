using Data;
using Data.World;
using LegendWorld.Data.Abilities;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class PowerScrollItem : ConsumableItem //Consumable
    {
        public CharacterPowerIdentity Ability { get { return (CharacterPowerIdentity)base.StackCount; } set { base.StackCount = (int)value; } }
        public PowerScrollItem()
        {
            //this.Identity = ItemIdentity.AbilityScoll;
            this.Weight = 100;
        }
        public override bool OnUse(Character usedBy, WorldState worldState)
        {
            if (usedBy.Learn(this.Ability))
            {
                this.StackCount = 0;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", this.Ability, this.Data.Identity);
        }
    }
}
