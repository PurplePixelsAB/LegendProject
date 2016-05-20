using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
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
