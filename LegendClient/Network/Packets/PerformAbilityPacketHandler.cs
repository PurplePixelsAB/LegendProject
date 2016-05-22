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

namespace WindowsClient.Net.Packets
{
    internal class PerformAbilityPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            PerformAbilityPacket incomingPacket = (PerformAbilityPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.CharacterId);
            if (mobileToUpdate == null)
            {
                worldState.MissingCharacters.Add(incomingPacket.CharacterId);
                return;
            }
            
            worldState.PerformAbility(incomingPacket.AbilityUsed, mobileToUpdate);
        }
    }
}
