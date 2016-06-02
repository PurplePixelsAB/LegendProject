using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Network.Packets
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(Packet packet) : this(new PacketStream(packet, 0, packet.Size))
        {

        }
        public PacketReader(PacketStream stream) : base(stream)
        {

        }

        public Int32? ReadNullableInt32()
        {
            Int32? value = null;
            bool hasValue = this.ReadBoolean();
            if (hasValue)
                value = this.ReadInt32();

            return value;
        }
        public byte? ReadNullableByte()
        {
            byte? value = null;
            bool hasValue = this.ReadBoolean();
            if (hasValue)
                value = this.ReadByte();

            return value;
        }
        public float? ReadNullableFloat()
        {
            float? value = null;
            bool hasValue = this.ReadBoolean();
            if (hasValue)
                value = this.ReadSingle();

            return value;
        }

        internal Point? ReadNullablePoint()
        {
            Point? value = null;
            bool hasValue = this.ReadBoolean();
            if (hasValue)
            {
                value = new Point(this.ReadInt32(), this.ReadInt32());
            }

            return value;
        }
    }
}
