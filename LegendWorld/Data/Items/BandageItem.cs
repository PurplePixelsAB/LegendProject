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
    public class BandageItem : ConsumableItem //Consumable
    {
        public BandageItem()
        {
            //this.Identity = ItemIdentity.AbilityScoll;
            this.Weight = 100;
            this.HealAmount = 10;
        }
        public byte HealAmount { get; protected set; }
        public override bool OnUse(Character usedBy, WorldState worldState)
        {
            if (this.StackCount <= 0)
                return false;

            if (usedBy.IsBusy)
                return false;

            usedBy.Health += this.HealAmount;
            this.StackCount--;

            if (this.StackCount <= 0)
            {
                worldState.RemoveItem(this);
            }

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.Data.Identity);
        }
    }
}
