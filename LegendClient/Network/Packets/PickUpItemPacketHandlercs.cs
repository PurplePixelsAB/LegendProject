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
using LegendWorld.Data.Items;
using LegendWorld.Data;

namespace WindowsClient.Net.Packets
{
    internal class PickUpItemPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            PickUpItemPacket incomingPacket = (PickUpItemPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.MobileId);
            IItem item = worldState.GetItem(incomingPacket.ItemId);

            if (mobileToUpdate == null)
            {
                worldState.MissingCharacters.Add(incomingPacket.MobileId);
            }

            if (mobileToUpdate != null && item != null)
            {
                if (item.Data.IsWorldItem)
                {
                    if (!mobileToUpdate.PickupItem(item))
                    {
                        //ToDo: Simulation missmatch, Force server Authority
                    }
                }
                else
                {
                    if (!mobileToUpdate.DropItem(item))
                    {

                    }
                }
            }
        }
    }
}
