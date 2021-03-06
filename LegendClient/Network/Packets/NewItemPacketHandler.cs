﻿using Network.Packets;
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
    internal class NewItemPacketHandler : ClientPacketHandler
    {
        protected override void OnHandle(IPacket packet, ClientWorldState worldState)
        {
            NewItemPacket incomingPacket = (NewItemPacket)packet;
            if (incomingPacket.ItemId == 0)
                return;

            Item item = worldState.GetItem(incomingPacket.ItemId);
            if (item == null)
            {
                ItemModel itemData = NetworkEngine.Instance.LoadItem(incomingPacket.ItemId);
                item = worldState.CreateItem(itemData);
                worldState.AddItem(item);
            }
        }
    }
}
