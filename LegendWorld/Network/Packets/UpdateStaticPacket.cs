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
        public UpdateStaticPacket() : base(PacketIdentity.UpdateStatic, byte.MaxValue * 14)
        {

        }
        public UpdateStaticPacket(List<GroundItem> itemsToUpdate) : base(PacketIdentity.UpdateStatic, itemsToUpdate.Count * 14)
        {
            this.Length = (byte)itemsToUpdate.Count;
            this.Id = new UInt16[this.Length];
            this.ItemId = new UInt16[this.Length];
            this.PositionX = new int[this.Length];
            this.PositionY = new int[this.Length];

            int i = 0;
            foreach (var posItem in itemsToUpdate)
            {
                this.Id[i] = posItem.Id;
                this.ItemId[i] = posItem.ItemId;
                this.PositionX[i] = posItem.Position.X;
                this.PositionY[i] = posItem.Position.Y;

                i++;
            }
        }

        public byte Length { get; set; }
        public UInt16[] Id { get; set; }
        public int[] PositionX { get; set; }
        public int[] PositionY { get; set; }

        public UInt16[] ItemId { get; set; }
        //public ItemLocation Location { get; set; }
        //public bool HasLocation { get { return this.Location == null; } }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.Length = packetReader.ReadByte();
            this.Id = new UInt16[this.Length];
            this.ItemId = new UInt16[this.Length];
            this.PositionX = new int[this.Length];
            this.PositionY = new int[this.Length];

            for (int i = 0; i < this.Length; i++)
            {
                this.Id[i] = packetReader.ReadUInt16();
                this.ItemId[i] = packetReader.ReadUInt16();
                this.PositionX[i] = packetReader.ReadInt32();
                this.PositionY[i] = packetReader.ReadInt32();
            }

            //bool hasLocation = packetReader.ReadBoolean();
            //if (hasLocation)
            //{
            //    this.Location = new ItemLocation();
            //    bool isInBag = packetReader.ReadBoolean();
            //    if (isInBag)
            //    {
            //        this.Location.BagId = packetReader.ReadUInt16();
            //    }
            //    else
            //    {
            //        this.Location.WorldMapId = packetReader.ReadUInt16();
            //        this.Location.WorldPosition = new Point(packetReader.ReadInt32(), packetReader.ReadInt32());
            //    }
            //}
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.Length);

            for (int i = 0; i < this.Length; i++)
            {
                packetWriter.Write(this.Id[i]);
                packetWriter.Write(this.ItemId[i]);
                packetWriter.Write(this.PositionX[i]);
                packetWriter.Write(this.PositionY[i]);
            }

            //packetWriter.Write(this.HasLocation);
            //if (this.HasLocation)
            //{
            //    packetWriter.Write(this.Location.IsInBag);
            //    if (this.Location.IsInBag)
            //    {
            //        packetWriter.Write(this.Location.BagId.Value);
            //    }
            //    else
            //    {
            //        packetWriter.Write(this.Location.WorldMapId.Value);
            //        packetWriter.Write(this.Location.WorldPosition.Value.X);
            //        packetWriter.Write(this.Location.WorldPosition.Value.Y);
            //    }
            //}
        }
    }
}
