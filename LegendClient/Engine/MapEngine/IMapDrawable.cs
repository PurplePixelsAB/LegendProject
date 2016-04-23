using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Engine.Textures;

namespace Engine.MapEngine
{
    public interface IMapDrawable
    {
        Vector2 Position { get; set; }
        Sprite Sprite { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
        float Layer { get; set; }
        bool Visible { get; set; }
    }
}
