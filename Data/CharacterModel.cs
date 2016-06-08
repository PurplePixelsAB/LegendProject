using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [DataContract]
    public class CharacterModel
    {
        public CharacterModel()
        {
            this.Health = 100;
            this.Energy = 100;
            //this.Powers = new List<CharacterPowerLearned>();
            this.Powers = new List<int>(7);
        }            
        
        [DataMember]
        public int Id {get; set; }

        [DataMember]
        public int MapId { get; set; }

        [DataMember]
        public int WorldX { get; set; }
        [DataMember]
        public int WorldY { get; set; }
        
        public Point WorldLocation { get { return new Point(this.WorldX, this.WorldY); } }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte Health { get; set; }

        [DataMember]
        public byte Energy { get; set; }

        //[DataMember]
        //public virtual List<CharacterPowerLearned> Powers { get; set; }

        [DataMember]
        public int InventoryId { get; set; }

        //[DataMember]
        //public virtual ItemData Inventory { get; set; }

        //[DataMember]
        //[ForeignKey("Holster")]
        //public int? HolsterId { get; set; }

        //[DataMember]
        //public virtual ItemData Holster { get; set; }

        [DataMember]
        public int? ArmorId { get; set; }

        //[DataMember]
        //public virtual ItemData Armor { get; set; }

        [DataMember]
        public int? LeftHandId { get; set; }

        //[DataMember]
        //public virtual ItemData LeftHand { get; set; }

        [DataMember]
        public int? RightHandId { get; set; }

        //[DataMember]
        //public virtual ItemData RightHand { get; set; }

        [DataMember]
        public IEnumerable<int> Powers { get; set; }
    }

    //[DataContract]
    //public class CharacterPower
    //{
    //    [DataMember]
    //    public int Id { get; set; }

    //    [DataMember]
    //    //[ForeignKey("Character")]
    //    public int CharacterID { get; set; }

    //    //public virtual CharacterData Character { get; set; }

    //    [DataMember]
    //    public int Power { get; set; }        
    //}
}
