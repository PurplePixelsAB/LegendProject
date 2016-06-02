using Data.World;
using LegendWorld.Data.Modifiers;
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
    class ChatMessagePacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            ChatMessagePacket packetToHandle = (ChatMessagePacket)packet;
            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (serverCharacter != null)
            {
                string ghostText = this.GhostText(packetToHandle.Message);
                serverCharacter.SentMessage();

                foreach (int mapCharacterId in worldState.Characters)
                {
                    ServerCharacter characterToUpdate = (ServerCharacter)worldState.GetCharacter(mapCharacterId);
                    NetState clientSendTo = characterToUpdate.Owner;
                    ChatMessagePacket packetToSend;

                    if (serverCharacter.IsDead && !characterToUpdate.IsDead)
                    {
                        packetToSend = new ChatMessagePacket(serverCharacter.Id, ghostText);
                    }
                    else
                    {
                        packetToSend = new ChatMessagePacket(serverCharacter.Id, packetToHandle.Message);
                    }

                    clientSendTo.Send(packetToSend);
                }
            }
        }

        private string GhostText(string message)
        {
            string ghostText = string.Empty;
            string[] words = message.Split(new char[] { ' ' });
            foreach (string word in words)
            {
                ghostText += " " + this.GhostWord(word.Length);
            }

            return ghostText;
        }

        private string GhostWord(int length)
        {
            string ghostText = string.Empty;
            for (int i = 0; i < length; i++)
            {
                ghostText += i % 3 == 0 ? "O" : "o";
            }

            return ghostText;
        }
    }
}
