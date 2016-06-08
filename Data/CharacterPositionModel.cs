using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CharacterPositionModel
    {
        public int CharacterId { get; set; }
        public int MapId { get; set; }
        public int WorldX { get; set; }
        public int WorldY { get; set; }
    }
}
