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
    internal class UseItemPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            UseItemPacket incomingPacket = (UseItemPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.MobileId);
            Item itemToUse = worldState.GetItem(incomingPacket.ItemId);

            if (mobileToUpdate == null)
            {
                worldState.MissingCharacters.Add(incomingPacket.MobileId);
            }

            if (mobileToUpdate != null && itemToUse != null)
            {
                if (itemToUse.IsWorldItem)
                {
                    if (!mobileToUpdate.PickupItem(itemToUse))
                    {
                        //ToDo: Simulation missmatch, Force server Authority
                    }
                }
                else
                {
                    if (itemToUse.Category == ItemCategory.Consumable)
                    {
                        ConsumableItem itemToConsume = (ConsumableItem)itemToUse;
                        if (!itemToConsume.Use(mobileToUpdate, worldState))
                        {
                            //ToDo: Simulation missmatch, Force server Authority
                        }
                    }
                    else if (itemToUse.Category == ItemCategory.Armor || itemToUse.Category == ItemCategory.Weapon)
                    {
                        if (!mobileToUpdate.Equip(itemToUse))
                        {
                            //ToDo: Sim missmatch, force server Auth
                        }
                    }
                }
            }
        }
    }
}
