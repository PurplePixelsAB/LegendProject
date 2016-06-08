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
        public CharacterPowerIdentity Ability { get { return (CharacterPowerIdentity)base.SubType; } set { base.SubType = (int)value; } }
        public PowerScrollItem()
        {
            this.Identity = ItemIdentity.PowerScoll;
            this.Weight = 100;
        }
        public override bool OnUse(Character usedBy, WorldState worldState)
        {
            if (usedBy.Learn(this.Ability))
            {
                //this.StackCount = 0;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            if (this.Count > 0)
                return string.Format("{1} {0} of {2}", this.Identity, this.Count, this.Ability);

            return string.Format("{0} of {1}", this.Identity, this.Ability);

        }
    }
}
