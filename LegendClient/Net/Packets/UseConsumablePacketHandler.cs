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

namespace WindowsClient.Net.Packets
{    
    internal class UseConsumablePacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            UseItemPacket incomingPacket = (UseItemPacket)packet;
            ClientCharacter mobileToUpdate = (ClientCharacter)worldState.GetCharacter(incomingPacket.MobileId);
            ConsumableItem itemToConsume = (ConsumableItem)worldState.GetItem(incomingPacket.ItemId);
            if (mobileToUpdate != null && itemToConsume != null)
            {
                if (itemToConsume.Use(mobileToUpdate, worldState))
                {
                    worldState.RemoveItem(itemToConsume);
                }
            }
        }
    }
}
