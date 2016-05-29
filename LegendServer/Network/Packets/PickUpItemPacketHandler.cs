using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Items;
using LegendWorld.Data;

namespace UdpServer.Network.Packets
{    
    internal class PickUpItemPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            PickUpItemPacket incomingPacket = (PickUpItemPacket)packet;
            //if (incomingPacket.MobileId != netState.WorldId)
            //{
            //    netState.SendError(0, string.Format("Invalid CMD: '{0},{1},{2}'.", incomingPacket.PacketId, incomingPacket.MobileId, incomingPacket.ItemId));
            //    return;
            //}

            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            IItem item = (IItem)worldState.GetItem(incomingPacket.ItemId);
            if (serverCharacter != null && item != null)
            {
                if (item.Data.IsWorldItem)
                {
                    if (serverCharacter.PickupItem(item))
                    {
                        worldState.SaveItem(item);
                        this.OnSuccessfulChange(serverCharacter.CurrentMapId, incomingPacket.MobileId, incomingPacket.ItemId, worldState);
                    }
                }
                else
                {
                    if (serverCharacter.DropItem(item))
                    {
                        worldState.SaveItem(item);
                        this.OnSuccessfulChange(serverCharacter.CurrentMapId, incomingPacket.MobileId, incomingPacket.ItemId, worldState);
                    }
                }
            }
        }

        private void OnSuccessfulChange(int mapId, int mobileId, int itemId, ServerWorldState worldState)
        {
            foreach (int characterId in worldState.GetMapCharacters(mapId))
            {
                if (characterId == mobileId)
                    continue;

                ServerCharacter characterToUpdate = ((ServerCharacter)worldState.GetCharacter(characterId));
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(new PickUpItemPacket(itemId, mobileId));
            }
        }
    }
}
