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
    internal class PerformAbilityPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            PerformAbilityPacket incomingPacket = (PerformAbilityPacket)packet;
            ServerCharacter mobileToUpdate = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            if (mobileToUpdate != null)
            {
                if (worldState.PerformAbility(incomingPacket.AbilityUsed, mobileToUpdate))
                {
                    foreach (int characterId in worldState.GetMapCharacters(mobileToUpdate.CurrentMapId))
                    {
                        if (characterId == mobileToUpdate.Id)
                            continue;

                        ServerCharacter characterToUpdate = ((ServerCharacter)worldState.GetCharacter(characterId));
                        NetState clientSendTo = characterToUpdate.Owner;

                        clientSendTo.Send(incomingPacket);
                    }
                }
            }
        }
    }
}
