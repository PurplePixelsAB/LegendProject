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
        public UseItemPacket(int itemId, int mobileId) : this()
        {
            this.ItemId = itemId;
            this.MobileId = mobileId;
        }

        public Int32 ItemId { get; set; }
        public Int32 MobileId { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.ItemId = packetReader.ReadInt32();
            this.MobileId = packetReader.ReadInt32();

        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.ItemId);
            packetWriter.Write(this.MobileId);
        }
    }
}
