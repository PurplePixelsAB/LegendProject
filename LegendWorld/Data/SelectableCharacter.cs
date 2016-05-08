using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    [DataContract]
    public class SelectableCharacter
    {
        [DataMember]
        public int CharacterId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
