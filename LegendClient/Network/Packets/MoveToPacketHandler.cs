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
    internal class MoveToPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            MoveToPacket incomingPacket = (MoveToPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.MobileId);
            if (mobileToUpdate == null)
            {
                worldState.MissingCharacters.Add(incomingPacket.MobileId);
                return;
            }
            
            mobileToUpdate.ServerMoveToRecived(new Point(incomingPacket.X, incomingPacket.Y));
        }
    }
}
