using Data;
using Data.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class AuthPacket : Packet
    {
        public AuthPacket() : base(PacketIdentity.Auth, 30)
        {

        }
        public AuthPacket(int sessionId) : this()
        {
            this.SessionId = sessionId;
            //this.CharacterId = (UInt16)selectedId;
        }

        //public UInt16 CharacterId { get; set; }
        public int SessionId { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.SessionId = packetReader.ReadInt32();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.SessionId);
        }
    }
}
