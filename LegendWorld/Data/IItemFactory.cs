using Data;
using LegendWorld.Data;

namespace Network
{
    public interface IItemFactory
    {
        Item CreateNew(ItemModel itemData);
    }
}