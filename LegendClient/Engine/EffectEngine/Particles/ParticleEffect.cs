using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.EffectEngine;
using Engine.EffectEngine.Light;

namespace Engine.EffectEngine.Particles
{
    public abstract class ParticleEffect
    {
        public string Sprite { get; set; }
        public Vector2 Position { get; set; }
        public LightSource EffectLight { get; set; }

        public float Size { get; set; }
        public float Spread { get; set; }
        public float MaxAge { get; set; }

        public int Particles { get; set; }
                       
        internal EffectManager Manager { get; set; }
        internal List<Particle> ParticleList = new List<Particle>();

        protected float BirthTime = 0;
        private bool IsCreated = false;
        
        public void Create(EffectManager manager, GameTime gameTime)
        {
            if (IsCreated)
                throw new Exception("Effect already created.");

            Manager = manager;
            BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            for (int i = 0; i < Particles; i++)
                AddParticle(gameTime);

            Manager.Effects.Add(this);

            if (EffectLight != null)
                Manager.LightSources.Add(EffectLight);

            IsCreated = true;
        }
        public void Kill()
        {
            if (Manager != null)
            {
                Manager.Effects.Remove(this);
                if (EffectLight != null)
                    Manager.LightSources.Remove(EffectLight);

                ParticleList.Clear();
            }
        }

        internal abstract void AddParticle(GameTime gameTime);
        internal abstract void Update(GameTime gameTime);
    }
}
