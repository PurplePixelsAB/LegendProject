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
    internal class UseItemPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            UseItemPacket incomingPacket = (UseItemPacket)packet;
            //if (incomingPacket.MobileId != netState.WorldId)
            //{
            //    netState.SendError(0, string.Format("Invalid CMD: '{0},{1},{2}'.", incomingPacket.PacketId, incomingPacket.MobileId, incomingPacket.ItemId));
            //    return;
            //}

            ServerCharacter serverCharacter = (ServerCharacter)worldState.GetCharacter(netState.WorldId);
            IItem itemToUse = (IItem)worldState.GetItem(incomingPacket.ItemId);
            if (serverCharacter != null && itemToUse != null)
            {
                if (itemToUse.Data.IsWorldItem)
                {
                    if (serverCharacter.Pickup(itemToUse))
                    {
                        worldState.SaveItem(itemToUse);
                        this.OnSuccessfulUse(serverCharacter.CurrentMapId, incomingPacket, worldState);
                    }
                }
                else
                {
                    if (itemToUse.Category == ItemCategory.Consumable)
                    {
                        ConsumableItem consumable = (ConsumableItem)itemToUse;
                        if (consumable.Use(serverCharacter, worldState))
                        {
                            worldState.SaveItem(itemToUse);
                            this.OnSuccessfulUse(serverCharacter.CurrentMapId, incomingPacket, worldState);
                        }
                    }
                    if (itemToUse.Category == ItemCategory.Armor || itemToUse.Category == ItemCategory.Weapon)
                    {
                        if (serverCharacter.Equip(itemToUse))
                        {
                            this.OnSuccessfulUse(serverCharacter.CurrentMapId, incomingPacket, worldState);
                        }
                    }
                }
            }
        }

        private void OnSuccessfulUse(int mapId, UseItemPacket incomingPacket, ServerWorldState worldState)
        {
            foreach (int characterId in worldState.GetMapCharacters(mapId))
            {
                if (characterId == incomingPacket.MobileId)
                    continue;

                ServerCharacter characterToUpdate = ((ServerCharacter)worldState.GetCharacter(characterId));
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(incomingPacket);
            }
        }
    }
}
