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
            this.Stats.Health = this.LatestData.Health;
            this.Stats.Energy = this.LatestData.Energy;
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
            LatestData.Energy = (byte)this.Stats.Energy;
            LatestData.Health = (byte)this.Stats.Health;
            LatestData.MapID = this.CurrentMapId;
            LatestData.WorldX = this.Position.X;
            LatestData.WorldY = this.Position.Y;
            if (this.RightHand != null)
            {
                //LatestData.RightHand = this.RightHand.Data;
                LatestData.RightHandID = this.RightHand.Data.ItemDataID;
            }
            if (this.LeftHand != null)
            {
                //LatestData.LeftHand = this.LeftHand.Data;
                LatestData.LeftHandID = this.LeftHand.Data.ItemDataID;
            }
            if (this.Armor != null)
            {
                //LatestData.Armor = this.Armor.Data;
                LatestData.ArmorID = this.Armor.Data.ItemDataID;
            }

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