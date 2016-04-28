namespace LegendWorld.Data.Items
{
    public abstract class ArmorItem : Item
    {
        public ArmorItem()
        {
            this.Category = ItemCategory.Armor;
        }
        public int Armor { get; set; }
    }
}