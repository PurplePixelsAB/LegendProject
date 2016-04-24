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
    internal class UpdateMobilePacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            UpdateMobilePacket updateMobilePacket = (UpdateMobilePacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(updateMobilePacket.MobileId);
            if (mobileToUpdate == null)
            {
                mobileToUpdate = new ClientCharacter();
                mobileToUpdate.Id = updateMobilePacket.MobileId;
                worldState.AddCharacter(mobileToUpdate);
            }


            mobileToUpdate.CheckServerPosition(0, new Point(updateMobilePacket.X.Value, updateMobilePacket.Y.Value));

            if (updateMobilePacket.Health.HasValue)
                mobileToUpdate.Health = updateMobilePacket.Health.Value;
            if (updateMobilePacket.Speed.HasValue)
                mobileToUpdate.MaxSpeed = (int)updateMobilePacket.Speed.Value;

            //mobileToUpdate.Up = updateMobilePacket.Up;
            //mobileToUpdate.Up = updateMobilePacket.Down;
            //mobileToUpdate.Up = updateMobilePacket.Left;
            //mobileToUpdate.Up = updateMobilePacket.Right;
        }
    }
}
