using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine.EffectEngine.Light
{
    public class LightSource
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Range { get; set; }
        public float Pulsate { get; set; }
        public string Sprite { get; set; }

        [ContentSerializerIgnore]
        internal EffectManager Manager { get; set; }
        public void Create(EffectManager manager)
        {
            this.Manager = manager;
            this.Manager.LightSources.Add(this);
        }
        public void Kill()
        {
            if (Manager != null)
                this.Manager.LightSources.Remove(this);
        }
    }
}
