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
        public AbilityIdentity Ability { get; set; }
        public bool IsConsumed { get; set; }
        public AbilityScrollItem()
        {
            //this.Name = "Ability Scroll";
            //this.IsStackable = false;
            this.Identity = ItemIdentity.AbilityScoll;
            this.Category = ItemCategory.Consumable;
            //this.Weight = 100;
        }
        public override void OnUse(Character usedBy)
        {
            if (this.IsConsumed)
                return;

            if (usedBy.Teach(this.Ability))
            {
                this.IsConsumed = true;
            }
        }
    }
}
