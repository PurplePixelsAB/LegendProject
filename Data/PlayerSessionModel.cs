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
    public class PlayerSessionModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public int CharacterID { get; set; }
    }
}