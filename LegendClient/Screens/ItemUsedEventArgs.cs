using LegendWorld.Data;

namespace LegendClient.Screens
{
    internal class ItemUsedEventArgs
    {
        private IItem item;

        public ItemUsedEventArgs(IItem itemUsed)
        {
            this.ItemUsed = itemUsed;
        }

        public IItem ItemUsed
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