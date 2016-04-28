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
using LegendWorld.Data;

namespace WindowsClient.Net.Packets
{
    internal class UpdateMapStaticsPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            UpdateStaticPacket updatePacket = (UpdateStaticPacket)packet;
            for (int i = 0; i < updatePacket.Length; i++)
            {
                GroundItem staticToUpdate = worldState.GetGroundItem(updatePacket.Id[i]); //(ClientCharacter)worldState.GetCharacter(updatePacket.MobileId);
                if (staticToUpdate == null && updatePacket.ItemId[i] != ushort.MinValue)
                {
                    staticToUpdate = new GroundItem();
                    staticToUpdate.Id = updatePacket.Id[i];
                    worldState.AddGroundItem(staticToUpdate);
                }
                else if (staticToUpdate != null && updatePacket.ItemId[i] == ushort.MinValue)
                {
                    worldState.RemoveGroundItem(staticToUpdate);
                    continue;
                }
                else if (updatePacket.ItemId[i] == ushort.MinValue)
                    continue;

                staticToUpdate.ItemId = updatePacket.ItemId[i];
                staticToUpdate.Position = new Point(updatePacket.PositionX[i], updatePacket.PositionY[i]);
            }
        }
    }
}
