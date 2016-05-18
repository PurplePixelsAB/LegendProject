using System;
using Data;
using LegendWorld.Data;
using Network;

namespace UdpServer
{
    internal class ServerItemFactory : IItemFactory
    {
        private ItemData.ItemIdentity identity;

        public ServerItemFactory(ItemData.ItemIdentity identity)
        {
            this.identity = identity;
        }

        public IItem CreateNew(ItemData itemData)
        {
            return new ServerItem(itemData);
        }
    }
}