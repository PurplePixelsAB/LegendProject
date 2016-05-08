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
    [DataContract]
    public class AbilityScrollItem : ConsumableItem //Consumable
    {
        [DataMember]
        public AbilityIdentity Ability { get { return (AbilityIdentity)base.StackCount; } set { base.StackCount = (int)value; } }
        public AbilityScrollItem()
        {
            this.Identity = ItemIdentity.AbilityScoll;
            this.Weight = 100;
        }
        public override bool OnUse(Character usedBy, WorldState worldState)
        {
            if (usedBy.Teach(this.Ability))
            {
                return true;
            }

            return false;
        }
    }
}
