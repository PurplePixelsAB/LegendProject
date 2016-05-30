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
    internal class ChatMessagePacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            ChatMessagePacket recivedPacket = (ChatMessagePacket)packet;
            worldState.AddChatMessage(recivedPacket.MobileId, recivedPacket.Message);
        }
    }
}
