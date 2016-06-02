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
    public class MoveItemPacket : Packet
    {
        public MoveItemPacket() : base(PacketIdentity.MoveItem, 256)
        {

        }
        public MoveItemPacket(int itemID, int moveToID) : this()
        {
            this.ItemID = itemID;
            this.MoveToID = moveToID;
        }
        public MoveItemPacket(int itemID, Point moveToPosition) : this()
        {
            this.ItemID = itemID;
            this.MoveToPosition = moveToPosition;
            this.MoveToID = null;
        }

        public int ItemID { get; set; }
        public int? MoveToID { get; set; }
        public Point? MoveToPosition { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.ItemID = packetReader.ReadInt32();
            this.MoveToID = packetReader.ReadNullableInt32();
            this.MoveToPosition = packetReader.ReadNullablePoint();

        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.ItemID);
            packetWriter.WriteNullable(this.MoveToID);
            packetWriter.WriteNullable(this.MoveToPosition);
        }
    }
    public class MoveItemServerPacket : MoveItemPacket
    {
        public MoveItemServerPacket() : base()
        {

        }
        public MoveItemServerPacket(int itemID, int charID, int moveToID) : base(itemID, moveToID)
        {
            this.CharacterID = charID;
        }
        public MoveItemServerPacket(int itemID, int charID, Point moveToPosition) : base(itemID, moveToPosition)
        {
            this.CharacterID = charID;
        }
        public int CharacterID { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.ItemID = packetReader.ReadInt32();
            this.CharacterID = packetReader.ReadInt32();
            this.MoveToID = packetReader.ReadInt32();

        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.ItemID);
            packetWriter.Write(this.CharacterID);
            packetWriter.WriteNullable(this.MoveToID);
            packetWriter.WriteNullable(this.MoveToPosition);
        }
    }
}
