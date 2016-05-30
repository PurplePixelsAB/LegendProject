using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using WindowsClient.World;
using WindowsClient.World.Mobiles;
using Microsoft.Xna.Framework;
using LegendWorld.Network.Packets;

namespace WindowsClient.Net.Packets
{
    internal class ChatStatusPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            ChatStatusPacket recivedPacket = (ChatStatusPacket)packet;
            worldState.SetChatState(recivedPacket.MobileId, recivedPacket.State);
        }
    }
}
