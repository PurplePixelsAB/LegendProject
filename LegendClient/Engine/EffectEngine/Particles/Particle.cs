using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.EffectEngine.Particles
{
    internal class Particle
    {
        internal Vector2 Position { get; set; }
        //internal Vector2 OrginalPosition { get; set; }

        internal Vector2 Accelaration { get; set; }
        internal Vector2 Direction { get; set; }

        internal float Scaling { get; set; }
        internal Color ModColor { get; set; }
    }
}
