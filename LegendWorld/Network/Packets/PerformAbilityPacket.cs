using Data;
using Data.World;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Network.Packets
{
    public class PerformAbilityPacket : Packet
    {
        public PerformAbilityPacket() : base(PacketIdentity.PerformAbility, 30)
        {

        }
        public PerformAbilityPacket(int characterId, CharacterPowerIdentity ability) : base(PacketIdentity.PerformAbility)
        {
            this.CharacterId = (ushort)characterId;
            this.AbilityUsed = ability;
        }

        public ushort CharacterId { get; set; }
        public CharacterPowerIdentity AbilityUsed { get; set; }

        public override void OnReadBuffer(PacketReader packetReader)
        {
            base.OnReadBuffer(packetReader);
            this.CharacterId = packetReader.ReadUInt16();
            this.AbilityUsed = (CharacterPowerIdentity)packetReader.ReadUInt16();
        }
        public override void OnWriteBuffer(PacketWriter packetWriter)
        {
            base.OnWriteBuffer(packetWriter);
            packetWriter.Write(this.CharacterId);
            packetWriter.Write((ushort)this.AbilityUsed);
        }
    }
}
