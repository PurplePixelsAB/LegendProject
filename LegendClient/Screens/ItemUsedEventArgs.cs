using LegendWorld.Data;

namespace LegendClient.Screens
{
    internal class ItemUsedEventArgs
    {
        private Item item;

        public ItemUsedEventArgs(Item itemUsed)
        {
            this.ItemUsed = itemUsed;
        }

        public Item ItemUsed
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }
    }
}