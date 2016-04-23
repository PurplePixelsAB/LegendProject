using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using Microsoft.Xna.Framework;

namespace UdpServer.Network.Packets
{


    internal class SwingPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, WorldServer worldState)
        {
            SwingPacket swingPacket = (SwingPacket)packet;
            ServerCharacter mobileToUpdate = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (mobileToUpdate != null)
            {
                worldState.PerformSwing(mobileToUpdate);
            }

        }
    }
}
