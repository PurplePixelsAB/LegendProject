using Network.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network
{
    public class Packet : IPacket
    {
        public static int DefaultBufferSize = 5120;
        public static PacketIdentity GetIdentity(byte[] buffer)
        {
            if (buffer == null)
                return PacketIdentity.Invalid;
            if (buffer.Length <= 0)
                return PacketIdentity.Invalid;

            byte packedByteId = buffer[0];
            return (PacketIdentity)packedByteId;
        }

        //public Packet()
        //{
        //    this.PacketId = PacketIdentity.Invalid;
        //    this.Buffer = new byte[Packet.BufferSize];
        //}
        public Packet(PacketIdentity packetId) : this(packetId, Packet.DefaultBufferSize)
        {
        }
        public Packet(PacketIdentity packetId, int size)
        {
            this.PacketId = packetId;
            this.Buffer = new byte[size];
            this.Size = size;
        }
        public Packet(byte[] buffer)
        {
            if (buffer == null)
                new ArgumentNullException("buffer is null");

            this.Buffer = buffer;
            this.Size = buffer.Length;

            this.PacketId = PacketIdentity.Invalid;
            if (buffer.Length > 0)
                this.PacketId = (PacketIdentity)this.Buffer[0];
        }

        public PacketIdentity PacketId
        {
            get; protected set;
        }
        //public UInt32 Tick { get; set; }
        public byte[] Buffer { get; set; }
        public int Size { get; set; }

        public virtual void OnReadBuffer(PacketReader packetReader)
        {
            //this.Tick = packetReader.ReadUInt32();
            this.PacketId = (PacketIdentity)packetReader.ReadByte();
        }
        public virtual void OnWriteBuffer(PacketWriter packetWriter)
        {
            //packetWriter.Write(this.Tick);
            packetWriter.Write((byte)this.PacketId);
        }

        public void ReadBuffer()
        {
            if (this.Buffer == null)
                return;

            using (PacketReader packetReader = new PacketReader(this))
            {
                this.OnReadBuffer(packetReader);
            }
        }
        public void WriteBuffer()
        {
            if (this.Buffer == null)
                return;

            using (PacketWriter packetWriter = new PacketWriter(this))
            {
                this.OnWriteBuffer(packetWriter);
                this.Size = (int)packetWriter.BaseStream.Length;
            }
        }
    }
}
