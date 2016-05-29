using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
{
    public class BloodEffect : ParticleEffect
    {
        public BloodEffect()
        {
        }

        public BloodEffect(Point drawPosition)
        {
            this.Position = drawPosition.ToVector2();
            this.Sprite = "BloodSplatter01";
            this.Particles = 1;
            this.Size = .2f;
            this.Spread = 0f;
            this.MaxAge = 120000;
        }

        internal override void AddParticle(GameTime gameTime)
        {
            Particle particle = new Particle();

            //particle.OrginalPosition = Position;
            particle.Position = Position; // particle.OrginalPosition;

            ////particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            ////particle.MaxAge = MaxAge;
            //particle.Scaling = 0.25f;
            //particle.ModColor = Color.White;

            //float particleDistance = (float)Manager.Random.NextDouble() * this.Spread; //Size;
            //Vector2 displacement = new Vector2(particleDistance, 0);
            //float angle = MathHelper.ToRadians(Manager.Random.Next(360));
            //displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));


            //particle.Direction = displacement * 2.0f;
            //particle.Accelaration = -particle.Direction;

            ParticleList.Add(particle);
        }

        internal override void Update(GameTime gameTime)
        {

        }

    }
}
