using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer.Network
{    internal abstract class ServerPacketHandler
    {
        private static ServerPacketHandler[] Handlers = new ServerPacketHandler[byte.MaxValue];
        public static void Register(PacketIdentity packetID, ServerPacketHandler packetHandler)
        {
            Handlers[(byte)packetID] = packetHandler;
        }

        public static ServerPacketHandler GetHandler(PacketIdentity packetID)
        {
            return Handlers[(byte)packetID];
        }

        protected abstract void OnHandle(IPacket packet, NetState netState, WorldServer worldState);

        public void Handle(IPacket packet, NetState netState, WorldServer worldState)
        {
            this.OnHandle(packet, netState, worldState);
        }
    }
}
