using Data;
using LegendWorld.Data;

namespace Network
{
    public interface IItemFactory
    {
        IItem CreateNew(ItemData itemData);
    }
}