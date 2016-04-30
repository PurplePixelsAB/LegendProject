using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Items;

namespace UdpServer.Network.Packets
{    
    internal class UseConsumablePacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, WorldServer worldState)
        {
            UseConsumablePacket incomingPacket = (UseConsumablePacket)packet;
            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            ConsumableItem itemToConsume = (ConsumableItem)worldState.GetItem(incomingPacket.ItemId);
            if (serverCharacter != null && itemToConsume != null)
            {
                if (itemToConsume.Use(serverCharacter))
                {
                    worldState.RemoveItem(itemToConsume);
                }
            }
        }
    }
}
