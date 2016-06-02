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
    internal class MoveItemPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            MoveItemServerPacket incomingPacket = (MoveItemServerPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.CharacterID);
            IItem item = worldState.GetItem(incomingPacket.ItemID);

            if (mobileToUpdate == null)
            {
                worldState.MissingCharacters.Add(incomingPacket.CharacterID);
            }
            else if (item != null)
            {
                if (incomingPacket.MoveToID.HasValue) //(item.Data.IsWorldItem)
                {
                    ContainerItem moveToContainer = (ContainerItem)worldState.GetItem(incomingPacket.MoveToID.Value);
                    if (!mobileToUpdate.MoveItem(item, moveToContainer))
                    {
                        //ToDo: Simulation missmatch, Force server Authority
                    }
                }
                else if (incomingPacket.MoveToPosition.HasValue)
                {
                    if (!mobileToUpdate.MoveItem(item, incomingPacket.MoveToPosition.Value))
                    {
                        //ToDo: Simulation missmatch, Force server Authority
                    }
                }
            }
        }
    }
}
