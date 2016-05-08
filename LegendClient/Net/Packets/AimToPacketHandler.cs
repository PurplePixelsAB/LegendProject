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
    internal class AimToPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            AimToPacket aimToPacket = (AimToPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(aimToPacket.MobileId);
            if (mobileToUpdate == null)
            {
                mobileToUpdate = new ClientCharacter();
                mobileToUpdate.Id = aimToPacket.MobileId;
                worldState.AddCharacter(mobileToUpdate);
            }
            
            mobileToUpdate.ServerAimToRecived(new Point(aimToPacket.X, aimToPacket.Y));
        }
    }
}
