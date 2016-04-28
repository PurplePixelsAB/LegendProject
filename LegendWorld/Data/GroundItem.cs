using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data
{
    public class GroundItem : IHasPosition
    {
        public ushort CurrentMapId { get; set; }

        public ushort Id { get; set; }

        public Point Position { get; set; }

        public ushort ItemId { get; set; }
    }
}
