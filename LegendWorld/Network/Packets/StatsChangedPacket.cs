using Data;
using Data.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class StatsChangedPacket : Packet
    {
        public StatsChangedPacket() : base(PacketIdentity.StatsChanged, 32) //48
        {

        }
        public StatsChangedPacket(int characterId, byte health, byte energy) : this()
        {
            this.MobileId = (ushort)characterId;
            //this.X = character.Position.X;
            //this.Y = character.Position.Y;
            //if (character.MovingToPosition != character.Position)
            //{
            //    this.MovingToX = character.MovingToPosition.X;
            //    this.MovingToY = character.MovingToPosition.Y;
            //}
            //this.AimToX = character.AimToPosition.X;
            //this.AimToY = character.AimToPosition.Y;
            this.Health = health;
            this.Energy = energy;
        }
        public StatsChangedPacket(ushort id) : base(PacketIdentity.StatsChanged, 5)
        {
            this.MobileId = id;
            //this.IsVisible = false;
        }

        public UInt16 MobileId { get; protected set; }
        //public bool IsVisible { get; set; }

        //public Int32? X { get; set; }
        //public Int32? Y { get; set; }
        //public Int32? MovingToX { get; set; }
        //public Int32? MovingToY { get; set; }
        //public Int32? AimToX { get; set; }
        //public Int32? AimToY { get; set; }

        //public byte? Speed { get; set; } //PixelsPerSec
        public byte? Health { get; protected set; }
        public byte? Energy { get; protected set; }
        //public Int32 X { get; set; }
        //public Int32 Y { get; set; }
        //public Int32 MovingToX { get; set; }
        //public Int32 MovingToY { get; set; }
        //public Int32 AimToX { get; set; }
        //public Int32 AimToY { get; set; }

        //public byte Speed { get; set; } //PixelsPerSec
        //public byte Health { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.MobileId = packetReader.ReadUInt16();

            //this.X = packetReader.ReadNullableInt32();
            //this.Y = packetReader.ReadNullableInt32();

            //this.MovingToX = packetReader.ReadNullableInt32();
            //this.MovingToY = packetReader.ReadNullableInt32();

            //this.AimToX = packetReader.ReadNullableInt32();
            //this.AimToY = packetReader.ReadNullableInt32();

            //this.Speed = packetReader.ReadNullableByte();
            this.Health = packetReader.ReadNullableByte();
            this.Energy = packetReader.ReadNullableByte();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.MobileId);

            //packetWriter.WriteNullable(this.X);
            //packetWriter.WriteNullable(this.Y);

            //packetWriter.WriteNullable(this.MovingToX);
            //packetWriter.WriteNullable(this.MovingToY);

            //packetWriter.WriteNullable(this.AimToX);
            //packetWriter.WriteNullable(this.AimToY);

            //packetWriter.WriteNullable(this.Speed);
            packetWriter.WriteNullable(this.Health);
            packetWriter.WriteNullable(this.Energy);
        }
    }
}
