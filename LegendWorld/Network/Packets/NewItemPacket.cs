using Data;
using Data.World;
using LegendWorld.Data;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class NewItemPacket : Packet
    {
        public NewItemPacket() : base(PacketIdentity.NewItem, 32)
        {

        }
        public NewItemPacket(int itemId) : this()
        {
            this.ItemId = itemId;
        }

        public Int32 ItemId { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.ItemId = packetReader.ReadInt32();

        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.ItemId);
        }
    }
}
