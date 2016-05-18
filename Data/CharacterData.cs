using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [DataContract]
    public class CharacterData
    {
        [Key]
        [DataMember]
        public int CharacterDataID {get; set; }

        [DataMember]
        public int MapID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual List<int> Powers { get; set; }
    }
}
