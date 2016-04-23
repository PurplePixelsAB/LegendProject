using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class PacketWriter : BinaryWriter
    {
        public PacketWriter(Packet packet) : this(new PacketStream(packet, 0, packet.Size))
        {
        }
        internal PacketWriter(PacketStream packetStream) : base(packetStream)
        {
        }

        public void WriteNullable(int? nullableValue)
        {
            this.Write(nullableValue.HasValue);
            if (nullableValue.HasValue)
                this.Write(nullableValue.Value);
        }
        public void WriteNullable(byte? nullableValue)
        {
            this.Write(nullableValue.HasValue);
            if (nullableValue.HasValue)
                this.Write(nullableValue.Value);
        }
        public void WriteNullable(float? nullableValue)
        {
            this.Write(nullableValue.HasValue);
            if (nullableValue.HasValue)
                this.Write(nullableValue.Value);
        }
    }
}
