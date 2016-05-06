using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace LegendWorld.Data
{
    public class GroundItem //: IHasPosition
    {
        public ushort CurrentMapId { get; set; }

        [Key]
        public int Id { get; set; }

        public Point Position { get; set; }

        public ushort ItemId { get; set; }
    }
}
