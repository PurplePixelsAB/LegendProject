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
            MoveToPacket moveToPacket = (MoveToPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(moveToPacket.MobileId);
            if (mobileToUpdate == null)
            {
                mobileToUpdate = new ClientCharacter();
                mobileToUpdate.Id = moveToPacket.MobileId;
                worldState.AddCharacter(mobileToUpdate);
            }
            
            mobileToUpdate.ServerMoveToRecived(new Point(moveToPacket.X, moveToPacket.Y));
        }
    }
}
