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
    public class MoveToPacket : Packet
    {
        public MoveToPacket() : base(PacketIdentity.MoveTo, 9)
        {

        }
        public MoveToPacket(Point moveTo) : this()
        {
            this.X = moveTo.X;
            this.Y = moveTo.Y;
        }
        
        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.Y = packetReader.ReadInt32();
            this.X = packetReader.ReadInt32();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.Y);
            packetWriter.Write(this.X);
        }
    }
}
