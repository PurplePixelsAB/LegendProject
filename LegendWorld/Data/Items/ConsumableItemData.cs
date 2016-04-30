using System;
using Data.World;

namespace LegendWorld.Data.Items
{
    public abstract class ConsumableItem : StackableItem
    {
        public ConsumableItem()
        {
            this.Category = ItemCategory.Consumable;
        }
        public abstract bool OnUse(Character usedBy);

        public bool Use(Character character)
        {
            if (character == null)
                return false;

            return this.OnUse(character);
        }
    }
}