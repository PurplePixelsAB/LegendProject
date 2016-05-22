using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
{
    public abstract class ParticleEffect
    {
        public string Sprite { get; set; }
        public Vector2 Position { get; set; }
        //public LightSource EffectLight { get; set; }

        public float Size { get; set; }
        public float Spread { get; set; }
        public float MaxAge { get; set; }

        public int Particles { get; set; }

        internal EffectManager Manager { get; set; }
        internal List<Particle> ParticleList = new List<Particle>();

        protected float BirthTime = 0;
        private bool IsCreated = false;

        internal void Create(EffectManager manager, GameTime gameTime)
        {
            if (IsCreated)
                throw new Exception("Effect already created.");

            Manager = manager;
            BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            for (int i = 0; i < Particles; i++)
                AddParticle(gameTime);

            //Manager.AddEffect(this);

            //if (EffectLight != null)
            //    Manager.LightSources.Add(EffectLight);

            IsCreated = true;
        }
        public void Kill()
        {
            if (Manager != null)
            {
                Manager.RemoveEffect(this);
                //if (EffectLight != null)
                //    Manager.LightSources.Remove(EffectLight);

                ParticleList.Clear();
            }
        }

        internal abstract void AddParticle(GameTime gameTime);
        internal virtual void Update(GameTime gameTime)
        {
            if (this.MaxAge > 0f)
            {
                float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
                for (int i = ParticleList.Count - 1; i >= 0; i--)
                {
                    float timeAlive = now - this.BirthTime;

                    if (timeAlive > this.MaxAge)
                    {
                        ParticleList.RemoveAt(i);
                    }
                }
            }
        }
    }
}
