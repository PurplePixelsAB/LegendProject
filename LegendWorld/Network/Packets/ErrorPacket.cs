using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Network.Packets
{
    public class ErrorPacket : Packet
    {
        public ErrorPacket() : base(PacketIdentity.Error, 1024)
        {

        }
        public ErrorPacket(int errorCode, string errorMessage) : this()
        {
            this.Code = errorCode;
            this.Message = errorMessage;
        }

        public int Code { get; set; }

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
            this.Code = packetReader.ReadInt32();
            this.Message = packetReader.ReadString();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.Code);
            packetWriter.Write(this.Message);
        }
    }
}
