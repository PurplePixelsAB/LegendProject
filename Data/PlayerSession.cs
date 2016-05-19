using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LegendWorld.Data
{
    [DataContract]
    public class PlayerSession
    {
        [Key]
        [DataMember]
        public int PlayerSessionID { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        [ForeignKey("Character")]
        public int CharacterID { get; set; }

        [DataMember]
        public virtual CharacterData Character { get; set; }
    }
}