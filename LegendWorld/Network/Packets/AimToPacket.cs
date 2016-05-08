using Data;
using Data.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class AimToPacket : Packet
    {
        public AimToPacket() : base(PacketIdentity.AimTo, 32)
        {

        }
        public AimToPacket(int mobileId, Point aimTo) : this()
        {
            this.MobileId = (ushort)mobileId;
            this.X = aimTo.X;
            this.Y = aimTo.Y;
        }

        public ushort MobileId { get; private set; }

        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.MobileId = packetReader.ReadUInt16();
            this.Y = packetReader.ReadInt32();
            this.X = packetReader.ReadInt32();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.MobileId);
            packetWriter.Write(this.Y);
            packetWriter.Write(this.X);
        }
    }
}
