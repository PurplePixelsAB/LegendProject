using Data.World;
using Microsoft.Xna.Framework;
using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer.Network.Packets
{
    internal class MoveToPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, WorldServer worldState)
        {
            MoveToPacket packetToHandle = (MoveToPacket)packet;
            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (serverCharacter != null)
            {
                serverCharacter.SetMoveToPosition(new Point(packetToHandle.X, packetToHandle.Y));
                worldState.UpdateEveryoneOfThisCharacter(serverCharacter);
            }
        }
    }
}
