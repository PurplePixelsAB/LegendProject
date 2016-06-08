using System;
using Data.World;
using System.Runtime.Serialization;
using Network;
using Data;

namespace LegendWorld.Data.Items
{
    public abstract class ConsumableItem : StackableItem
    {
        public ConsumableItem()
        {
            this.Category = ItemCategory.Consumable;
        }
        public abstract bool OnUse(Character usedBy, WorldState worldState);
        //public ItemData Data { get; set; }
        //public ItemCategory Category { get; protected set; }
        //public int Weight { get; protected set; }

        public bool Use(Character character, WorldState worldState)
        {
            if (character == null)
                return false;
            if (character.IsDead)
                return false;
            if (character.IsBusy)
                return false;

            bool result = this.OnUse(character, worldState);
            if (result)
            {
                this.Count--;
                if (this.Count <= 0)
                {
                    worldState.RemoveItem(this);
                }
            }

            return result;
        }
        public override string ToString()
        {
            if (this.Count > 0)
                return string.Format("{1} {0}", this.Identity, this.Count);

            return string.Format("{0}", this.Identity);

        }
    }
}