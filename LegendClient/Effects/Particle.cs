using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
{
    public class Particle
    {
        public Vector2 Position { get; set; }
        //internal Vector2 OrginalPosition { get; set; }

        public Vector2 Accelaration { get; set; }
        public Vector2 Direction { get; set; }

        public float Scale { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; internal set; }
    }
}
