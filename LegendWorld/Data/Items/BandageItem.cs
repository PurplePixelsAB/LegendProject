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
            this.Identity = ItemIdentity.Bandage;
            this.Weight = 100;
            this.HealAmount = 10;
        }
        public byte HealAmount { get; protected set; }
        public override bool OnUse(Character usedBy, WorldState worldState)
        {
            if (this.Count <= 0)
                return false;

            if (usedBy.IsBusy)
                return false;

            usedBy.Stats.Health += this.HealAmount;
            this.Count--;

            if (this.Count <= 0)
            {
                worldState.RemoveItem(this);
            }

            return true;
        }
    }
}
