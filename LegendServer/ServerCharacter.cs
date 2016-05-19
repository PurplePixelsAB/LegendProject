using System;
using Data;
using Data.World;
using Microsoft.Xna.Framework;

namespace UdpServer
{
    internal class ServerCharacter : Character, IServerCharacter
    {
        public NetState Owner { get; set; }
        public CharacterData LatestData { get; set; }

        public ServerCharacter(CharacterData characterData)
        {
            this.LatestData = characterData;
            this.Id = this.LatestData.CharacterDataID;
            this.Position = this.LatestData.WorldLocation;
            this.AimToPosition = this.Position;
            this.MovingToPosition = this.Position;
            this.Health = this.LatestData.Health;
            this.Energy = this.LatestData.Energy;
        }
        internal CharacterData GetData()
        {
            CharacterData characterData = new CharacterData();
            characterData.CharacterDataID = this.Id;
            characterData.Energy = this.Energy;
            characterData.Health = this.Health;
            characterData.MapID = this.CurrentMapId;
            characterData.WorldX = this.Position.X;
            characterData.WorldY = this.Position.Y;

            this.LatestData = characterData;
            return characterData;
        }
    }
}