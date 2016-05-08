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
    public class UseItemPacket : Packet
    {
        public UseItemPacket() : base(PacketIdentity.UseItem, 32)
        {

        }
        public UseItemPacket(ushort itemId, ushort mobileId) : this()
        {
            this.ItemId = itemId;
        }

        public UInt16 ItemId { get; set; }
        public UInt16 MobileId { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.ItemId = packetReader.ReadUInt16();
            this.MobileId = packetReader.ReadUInt16();

        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.ItemId);
            packetWriter.Write(this.MobileId);
        }
    }
}
