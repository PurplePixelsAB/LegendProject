using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LegendWorld.Data
{
    [DataContract]
    public class PlayerSession
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CharacterId { get; set; }
        [DataMember]
        public string ClientAddress { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
    }
}