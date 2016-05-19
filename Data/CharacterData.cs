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
    public class CharacterData
    {
        public CharacterData()
        {
            this.Health = 100;
            this.Energy = 100;
        }            

        [Key]
        [DataMember]
        public int CharacterDataID {get; set; }

        [DataMember]
        public int MapID { get; set; }

        [DataMember]
        public int WorldX { get; set; }
        [DataMember]
        public int WorldY { get; set; }

        [NotMapped]
        public Point WorldLocation { get { return new Point(this.WorldX, this.WorldY); } }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte Health { get; set; }

        [DataMember]
        public byte Energy { get; set; }

        [DataMember]
        public virtual List<int> Powers { get; set; }

        [DataMember]
        [ForeignKey("Inventory")]
        public int InventoryID { get; set; }

        [DataMember]
        public virtual ItemData Inventory { get; set; }

        [DataMember]
        [ForeignKey("Holster")]
        public int? HolsterID { get; set; }

        [DataMember]
        public virtual ItemData Holster { get; set; }

        [DataMember]
        [ForeignKey("Armor")]
        public int? ArmorID { get; set; }

        [DataMember]
        public virtual ItemData Armor { get; set; }

        [DataMember]
        [ForeignKey("LeftHand")]
        public int? LeftHandID { get; set; }

        [DataMember]
        public virtual ItemData LeftHand { get; set; }

        [DataMember]
        [ForeignKey("RightHand")]
        public int? RightHandID { get; set; }

        [DataMember]
        public virtual ItemData RightHand { get; set; }
    }
}
