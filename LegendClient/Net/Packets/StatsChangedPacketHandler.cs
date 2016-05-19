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
    internal class StatsChangedPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            StatsChangedPacket statsChangedPacket = (StatsChangedPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(statsChangedPacket.MobileId);
            if (mobileToUpdate == null)
            {
                //mobileToUpdate = new ClientCharacter();
                //mobileToUpdate.Id = statsChangedPacket.MobileId;
                //worldState.AddCharacter(mobileToUpdate);
                return;
            }

            mobileToUpdate.ServerStatsRecived(statsChangedPacket.Health, statsChangedPacket.Energy);
        }
    }
}
