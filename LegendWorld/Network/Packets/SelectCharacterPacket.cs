using Data;
using Data.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class SelectCharacterPacket : Packet
    {
        public SelectCharacterPacket() : base(PacketIdentity.SelectCharacter, 5)
        {

        }
        public SelectCharacterPacket(int selectedId) : this()
        {
            this.CharacterId = (UInt16)selectedId;
        }

        public UInt16 CharacterId { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.CharacterId = packetReader.ReadUInt16();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.CharacterId);
        }
    }
}
