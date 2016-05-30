using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Network.Packets
{
    public class ChatStatusPacket : Packet
    {
        public ChatStatusPacket() : base(PacketIdentity.ChatStatus, 48)
        {

        }
        public ChatStatusPacket(int mobileId, bool state) : this()
        {
            this.MobileId = mobileId;
            this.State = state;
        }

        public int MobileId { get; set; }
        public bool State { get; private set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.MobileId = packetReader.ReadInt32();
            this.State = packetReader.ReadBoolean();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.MobileId);
            packetWriter.Write(this.State);
        }
    }
}
