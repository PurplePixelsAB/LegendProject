using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
{
    public class TestEffect : ParticleEffect
    {
        public TestEffect()
        {
            this.Sprite = "StarLight";
            this.Particles = 1;
            this.Size = .2f;
            this.Spread = 0f;
        }

        internal override void AddParticle(GameTime gameTime)
        {
            Particle particle = new Particle();

            //particle.OrginalPosition = Position;
            particle.Position = Position; // particle.OrginalPosition;

            //particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            //particle.MaxAge = MaxAge;
            particle.Scale = 0.25f;
            particle.Color = Color.White;

            float particleDistance = (float)Manager.Random.NextDouble() * this.Spread; //Size;
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(Manager.Random.Next(360));
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));


            particle.Direction = displacement * 2.0f;
            particle.Accelaration = -particle.Direction;

            ParticleList.Add(particle);
        }

        internal override void Update(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            for (int i = ParticleList.Count - 1; i >= 0; i--)
            {
                Particle particle = ParticleList[i];
                float timeAlive = now - this.BirthTime;

                if (timeAlive > this.MaxAge)
                {
                    ParticleList.RemoveAt(i);
                }
                else
                {
                    float relAge = timeAlive / this.MaxAge;
                    particle.Position = 0.5f * particle.Accelaration * relAge * relAge + particle.Direction * relAge + this.Position; //particle.OrginalPosition;

                    float invAge = 1.0f - relAge;
                    particle.Color = new Color(new Vector4(invAge, invAge, invAge, invAge));

                    Vector2 positionFromCenter = particle.Position - this.Position; //particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    particle.Scale = ((50.0f + distance) / 200.0f) * this.Size;

                    ParticleList[i] = particle;
                }
            }
        }

    }
}
