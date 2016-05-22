using System.Runtime.Serialization;

namespace Data
{
    [DataContract]
    public class SelectableCharacter
    {
        [DataMember]
        public int CharacterId { get; set; }

        [DataMember]
        public int MapId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
