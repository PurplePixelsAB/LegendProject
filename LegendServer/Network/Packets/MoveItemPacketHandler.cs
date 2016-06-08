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
    internal class MoveItemPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            MoveItemPacket incomingPacket = (MoveItemPacket)packet;
            //if (incomingPacket.MobileId != netState.WorldId)
            //{
            //    netState.SendError(0, string.Format("Invalid CMD: '{0},{1},{2}'.", incomingPacket.PacketId, incomingPacket.MobileId, incomingPacket.ItemId));
            //    return;
            //}

            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            Item item = (Item)worldState.GetItem(incomingPacket.ItemID);
            if (serverCharacter != null && item != null)
            {
                if (incomingPacket.MoveToID.HasValue) //item.Data.IsWorldItem && 
                {
                    int containerID = incomingPacket.MoveToID.Value;
                    ContainerItem moveTo = (ContainerItem)worldState.GetItem(containerID);
                    if (moveTo != null)
                    {
                        if (serverCharacter.MoveItem(item, moveTo))
                        {
                            worldState.SaveItemPosition(item);
                            this.OnSuccessfulChange(serverCharacter.CurrentMapId, serverCharacter.Id, containerID, incomingPacket.ItemID, worldState);
                        }
                    }
                }
                else if (incomingPacket.MoveToPosition.HasValue)
                {
                    Point moveToPostion = incomingPacket.MoveToPosition.Value;
                    if (serverCharacter.MoveItem(item, moveToPostion))
                    {
                        worldState.SaveItemPosition(item);
                        this.OnSuccessfulChange(serverCharacter.CurrentMapId, serverCharacter.Id, moveToPostion, incomingPacket.ItemID, worldState);
                    }
                }
            }
        }

        private void OnSuccessfulChange(int currentMapId, int mobileId, Point moveToPostion, int itemID, ServerWorldState worldState)
        {
            foreach (int characterId in worldState.GetMapCharacters(currentMapId))
            {
                if (characterId == mobileId)
                    continue;

                ServerCharacter characterToUpdate = ((ServerCharacter)worldState.GetCharacter(characterId));
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(new MoveItemServerPacket(itemID, mobileId, moveToPostion));
            }
        }

        private void OnSuccessfulChange(int mapId, int mobileId, int containerID, int itemId, ServerWorldState worldState)
        {
            foreach (int characterId in worldState.GetMapCharacters(mapId))
            {
                if (characterId == mobileId)
                    continue;

                ServerCharacter characterToUpdate = ((ServerCharacter)worldState.GetCharacter(characterId));
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(new MoveItemServerPacket(itemId, mobileId, containerID));
            }
        }
    }
}
