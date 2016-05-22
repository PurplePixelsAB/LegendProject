using System;
using Data;
using Data.World;
using Microsoft.Xna.Framework;
using System.Linq;

namespace UdpServer
{
    internal class ServerCharacter : Character, IServerCharacter
    {
        public NetState Owner { get; set; }
        public CharacterData LatestData { get; set; }
        public bool HasChanged { get; set; }

        public ServerCharacter(CharacterData characterData) : base(characterData.CharacterDataID)
        {
            this.LatestData = characterData;
            //this.Id = this.LatestData.CharacterDataID;
            this.Position = this.LatestData.WorldLocation;
            this.AimToPosition = this.Position;
            this.MovingToPosition = this.Position;
            this.Health = this.LatestData.Health;
            this.Energy = this.LatestData.Energy;
            foreach (var pwr in this.LatestData.Powers)
            {
                this.Powers.Add(pwr.Power);
            }

            //this.Powers = this.LatestData.Powers.Select(pwr => pwr.Power).ToList();
        }
        internal CharacterData GetData()
        {
            //CharacterData characterData = new CharacterData();
            //characterData.CharacterDataID = this.Id;
            //characterData.InventoryID = this.
            LatestData.Energy = (byte)this.Energy;
            LatestData.Health = (byte)this.Health;
            LatestData.MapID = this.CurrentMapId;
            LatestData.WorldX = this.Position.X;
            LatestData.WorldY = this.Position.Y;

            return LatestData;
        }
        
        protected override void OnPowerLearning(CharacterPowerIdentity power)
        {
            //base.OnPowerLearning(power);
            this.LatestData.Powers.Add(new CharacterPowerLearned() { Power = power });
            HasChanged = true;
        }
    }
}