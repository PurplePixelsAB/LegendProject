namespace LegendWorld.Data.Items
{
    public class StackableItem : Item
    {
        public StackableItem()
        {
            this.Category = ItemCategory.Stackable;
        }
        public uint StackCount { get; set; }
    }
}