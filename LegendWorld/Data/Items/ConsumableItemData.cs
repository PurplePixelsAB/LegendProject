using Data.World;

namespace LegendWorld.Data.Items
{
    public abstract class ConsumableItem : StackableItem
    {
        public ConsumableItem()
        {
            this.Category = ItemCategory.Consumable;
        }
        public abstract void OnUse(Character usedBy);
    }
}