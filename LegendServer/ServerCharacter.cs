using System;
using Data;
using Data.World;
using Microsoft.Xna.Framework;
using System.Linq;
using LegendWorld.Data;

namespace UdpServer
{
    internal class ServerCharacter : Character, IServerCharacter
    {
        public NetState Owner { get; set; }
        //public CharacterModel LatestData { get; set; }
        public bool HasChanged { get; set; }

        public ServerCharacter(CharacterModel characterData) : base(characterData.Id)
        {
            //this.LatestData = characterData;
            //this.Id = this.LatestData.CharacterDataID;
            this.Position = characterData.WorldLocation;
            this.AimToPosition = this.Position;
            this.MovingToPosition = this.Position;
            this.Stats.Health = characterData.Health;
            this.Stats.Energy = characterData.Energy;
            //foreach (var pwr in this.LatestData.Powers)
            //{
            //    this.Powers.Add(pwr.Power);
            //}

            //this.Powers = this.LatestData.Powers.Select(pwr => pwr.Power).ToList();
        }
        //internal CharacterModel GetData()
        //{
        //    //CharacterData characterData = new CharacterData();
        //    //characterData.CharacterDataID = this.Id;
        //    //characterData.InventoryID = this.
        //    LatestData.Energy = (byte)this.Stats.Energy;
        //    LatestData.Health = (byte)this.Stats.Health;
        //    LatestData.MapId = this.CurrentMapId;
        //    LatestData.WorldX = this.Position.X;
        //    LatestData.WorldY = this.Position.Y;
        //    if (this.RightHand != null)
        //    {
        //        //LatestData.RightHand = this.RightHand.Data;
        //        LatestData.RightHandId = this.RightHand.Id;
        //    }
        //    if (this.LeftHand != null)
        //    {
        //        //LatestData.LeftHand = this.LeftHand.Data;
        //        LatestData.LeftHandId = this.LeftHand.Id;
        //    }
        //    if (this.Armor != null)
        //    {
        //        //LatestData.Armor = this.Armor.Data;
        //        LatestData.ArmorId = this.Armor.Id;
        //    }

        //    return LatestData;
        //}
        
        protected override void OnPowerLearning(CharacterPowerIdentity power)
        {
            base.OnPowerLearning(power);
            //this.LatestData.Powers.Add(new CharacterPowerLearned() { Power = power, CharacterID = this.LatestData.Id, Character = this.LatestData });
            HasChanged = true;
        }
    }
}