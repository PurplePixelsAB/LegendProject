using Microsoft.Xna.Framework;
using System;

namespace Engine.EffectEngine.LightEngine
{
    internal class DayCycle
    {
        public TimeSpan BirthTime { get; internal set; }
        public Color Color { get; internal set; }
        public int CycleTime { get; internal set; }
        public string Name { get; internal set; }
    }
}