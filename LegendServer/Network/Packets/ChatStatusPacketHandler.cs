using Data.World;
using LegendWorld.Network.Packets;
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
    class ChatStatusPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            ChatStatusPacket packetToHandle = (ChatStatusPacket)packet;
            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (serverCharacter != null)
            {
                foreach (int mapCharacterId in worldState.Characters) 
                {
                    ServerCharacter characterToUpdate = (ServerCharacter)worldState.GetCharacter(mapCharacterId);
                    NetState clientSendTo = characterToUpdate.Owner;
                    ChatStatusPacket packetToSend;

                    if (serverCharacter.IsDead && !characterToUpdate.IsDead)
                    {
                        packetToSend = new ChatStatusPacket(serverCharacter.Id, packetToHandle.State);
                    }
                    else
                    {
                        packetToSend = new ChatStatusPacket(serverCharacter.Id, packetToHandle.State);
                    }

                    clientSendTo.Send(packetToSend);
                }
            }
        }
    }
}
