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
    class AimToPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            AimToPacket packetToHandle = (AimToPacket)packet;
            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (serverCharacter != null)
            {
                serverCharacter.SetAimToPosition(new Point(packetToHandle.X, packetToHandle.Y));
                worldState.SendStatChangeToMapCharacters(serverCharacter);                
            }
        }
    }
}
