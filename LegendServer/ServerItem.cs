using System;
using Data;
using LegendWorld.Data;

namespace UdpServer
{
    internal class ServerItem : IItem
    {
        public ServerItem(ItemData itemData)
        {
            this.Data = itemData;
        }

        public ItemData Data { get; set; }
        public ItemCategory Category { get; set; }
        public int Weight { get; set; }
    }
}