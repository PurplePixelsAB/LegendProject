using Data;
using Data.World;
using LegendWorld.Data;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class UpdateStaticPacket : Packet
    {
        public UpdateStaticPacket() : base(PacketIdentity.UpdateStatic, 5)
        {

        }
        public UpdateStaticPacket(int updateId) : this()
        {
            this.Id = (UInt16)updateId;
        }

        public UInt16 Id { get; set; }
        public ItemLocation Location { get; set; }
        public bool HasLocation { get { return this.Location == null; } }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.Id = packetReader.ReadUInt16();
            bool hasLocation = packetReader.ReadBoolean();
            if (hasLocation)
            {
                this.Location = new ItemLocation();
                bool isInBag = packetReader.ReadBoolean();
                if (isInBag)
                {
                    this.Location.BagId = packetReader.ReadUInt16();
                }
                else
                {
                    this.Location.WorldMapId = packetReader.ReadUInt16();
                    this.Location.WorldPosition = new Point(packetReader.ReadInt32(), packetReader.ReadInt32());
                }
            }
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.Id);
            packetWriter.Write(this.HasLocation);
            if (this.HasLocation)
            {
                packetWriter.Write(this.Location.IsInBag);
                if (this.Location.IsInBag)
                {
                    packetWriter.Write(this.Location.BagId.Value);
                }
                else
                {
                    packetWriter.Write(this.Location.WorldMapId.Value);
                    packetWriter.Write(this.Location.WorldPosition.Value.X);
                    packetWriter.Write(this.Location.WorldPosition.Value.Y);
                }
            }
        }
    }
}
