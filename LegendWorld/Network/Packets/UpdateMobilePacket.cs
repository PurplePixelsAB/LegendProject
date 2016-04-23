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
    public class UpdateMobilePacket : Packet
    {
        public UpdateMobilePacket() : base(PacketIdentity.UpdateMobile, 200) //48
        {

        }
        public UpdateMobilePacket(Character character) : this()
        {
            this.MobileId = character.Id;
            this.X = character.Position.X;
            this.Y = character.Position.Y;
            if (character.MovingToPosition != character.Position)
            {
                this.MovingToX = character.MovingToPosition.X;
                this.MovingToY = character.MovingToPosition.Y;
            }
            this.AimToX = character.AimToPosition.X;
            this.AimToY = character.AimToPosition.Y;
            this.Speed = (byte)character.MaxSpeed; //mobile.Speed;
            this.Health = character.Health;
            //this.Up = character.Up;
            //this.Down = character.Down;
            //this.Left = character.Left;
            //this.Right = character.Right;
        }

        public Int32 MobileId { get; set; }

        public Int32? X { get; set; }
        public Int32? Y { get; set; }
        public Int32? MovingToX { get; set; }
        public Int32? MovingToY { get; set; }
        public Int32? AimToX { get; set; }
        public Int32? AimToY { get; set; }

        public byte? Speed { get; set; } //PixelsPerSec
        public byte? Health { get; set; }
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
            this.MobileId = packetReader.ReadInt32();

            this.X = packetReader.ReadNullableInt32();
            this.Y = packetReader.ReadNullableInt32();

            this.MovingToX = packetReader.ReadNullableInt32();
            this.MovingToY = packetReader.ReadNullableInt32();

            this.AimToX = packetReader.ReadNullableInt32();
            this.AimToY = packetReader.ReadNullableInt32();

            this.Speed = packetReader.ReadNullableByte();
            this.Health = packetReader.ReadNullableByte();
            //this.X = packetReader.ReadInt32();
            //this.Y = packetReader.ReadInt32();

            //this.MovingToX = packetReader.ReadInt32();
            //this.MovingToY = packetReader.ReadInt32();

            //this.AimToX = packetReader.ReadInt32();
            //this.AimToY = packetReader.ReadInt32();

            //this.Speed = packetReader.ReadByte();
            //this.Health = packetReader.ReadByte();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.MobileId);

            packetWriter.WriteNullable(this.X);
            packetWriter.WriteNullable(this.Y);

            packetWriter.WriteNullable(this.MovingToX);
            packetWriter.WriteNullable(this.MovingToY);

            packetWriter.WriteNullable(this.AimToX);
            packetWriter.WriteNullable(this.AimToY);

            packetWriter.WriteNullable(this.Speed);
            packetWriter.WriteNullable(this.Health);

            //packetWriter.Write(this.X);
            //packetWriter.Write(this.Y);

            //packetWriter.Write(this.MovingToX);
            //packetWriter.Write(this.MovingToY);

            //packetWriter.Write(this.AimToX);
            //packetWriter.Write(this.AimToY);

            //packetWriter.Write(this.Speed);
            //packetWriter.Write(this.Health);
        }
    }
}
