using System;
using Engine.Textures;
using Engine.EffectEngine.Light;

namespace Engine.MapEngine
{
    public interface IMap
    {
        string Name { get; set; }
        SpriteSheet[] Sprites { get; set; }
        Sprite Base { get; set; }
        IMapDrawable[][] LayerDrawing { get; set; }
        LightSource[] LightSources { get; set; }
    }
}
