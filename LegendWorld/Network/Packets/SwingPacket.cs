using Data;
using Data.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class SwingPacket : Packet
    {
        public SwingPacket() : base(PacketIdentity.PerformSwing)
        {

        }
        public SwingPacket(int selectedId, AbilityIdentity ability) : base(PacketIdentity.PerformSwing)
        {
            this.CharacterId = selectedId;
        }

        public int CharacterId { get; set; }
        public AbilityIdentity AbilityUsed { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.CharacterId = packetReader.ReadInt32();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.CharacterId);
        }
    }
}
