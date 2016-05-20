using LegendWorld.Data;

namespace LegendClient.Screens
{
    internal class ItemUsedEventArgs
    {
        private IItem item;

        public ItemUsedEventArgs(IItem itemUsed, bool isWorldItem)
        {
            this.ItemUsed = itemUsed;
            this.IsWorldItem = isWorldItem;
        }

        public bool IsWorldItem { get; private set; }

        public IItem ItemUsed
        {
            get
            {
                return item;
            }

            private set
            {
                item = value;
            }
        }

    }
}