using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class PacketStream : MemoryStream
    {
        public PacketStream(Packet packet, int index, int count) : base(packet.Buffer, index, count)
        {

        }

        internal PacketStream(Packet packet) : base (packet.Buffer)
        {

        }
    }
}
