using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Network.Packets
{
    public class ChatMessagePacket : Packet
    {
        public ChatMessagePacket() : base(PacketIdentity.ChatMessage, 1024)
        {

        }
        public ChatMessagePacket(int mobileId, string message) : this()
        {
            this.MobileId = mobileId;
            this.Message = message;
        }

        public int MobileId { get; set; }

        private string message = string.Empty;
        public string Message
        {
            get { return message; }
            set
            {
                string newMessage = value;
                if (string.IsNullOrWhiteSpace(newMessage))
                    message = string.Empty;
                else
                {
                    newMessage = newMessage.Trim();
                    if (newMessage.Length > byte.MaxValue)
                    {
                        newMessage = newMessage.Substring(0, byte.MaxValue);
                    }
                    message = newMessage;
                }
            }
        }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.MobileId = packetReader.ReadInt32();
            this.Message = packetReader.ReadString();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.MobileId);
            packetWriter.Write(this.Message);
        }
    }
}
