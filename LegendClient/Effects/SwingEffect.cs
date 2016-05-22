﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendWorld.Data.Items;

namespace LegendClient.Effects
{
    public class SwingEffect : ParticleEffect
    {
        Point aimAt;

        public SwingEffect(Point position, Point aimAt, bool isBow)
        {
            this.Position = position.ToVector2();            
            this.Sprite = "Swing";
            this.Particles = 1;
            this.Size = .2f;
            this.Spread = 0f;
            this.MaxAge = 2000;

            this.aimAt = aimAt;
        }

        internal override void AddParticle(GameTime gameTime)
        {
            Particle particle = new Particle();

            //particle.OrginalPosition = Position;
            particle.Position = Position; // particle.OrginalPosition;
            //particle.Rotation = rotation;
            ////particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            ////particle.MaxAge = MaxAge;
            particle.Scale = 1f;
            particle.Color = Color.White;

            //float particleDistance = (float)Manager.Random.NextDouble() * this.Spread; //Size;
            //Vector2 displacement = new Vector2(particleDistance, 0);
            //float angle = MathHelper.ToRadians(Manager.Random.Next(360));
            //displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));
            //particle.Direction = displacement * 2.0f;
            //particle.Accelaration = -particle.Direction;

            Vector2 direction = this.Position - this.aimAt.ToVector2();
            direction.Normalize();
            particle.Direction = direction;
            particle.Accelaration = new Vector2(-3f, -3f);

            ParticleList.Add(particle);
        }

        internal override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
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
                    particle.Position += particle.Accelaration * particle.Direction; //0.5f * particle.Accelaration * relAge * relAge + particle.Direction * relAge + this.Position; //particle.OrginalPosition;

                    float invAge = 1.0f - relAge;
                    particle.Color = particle.Color * invAge;
                    //particle.Color = new Color(new Vector4(invAge, invAge, invAge, invAge));

                    Vector2 positionFromCenter = particle.Position - this.Position; //particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    //particle.Scale = ((50.0f + distance) / 200.0f) * this.Size;

                    ParticleList[i] = particle;
                }
            }
        }

    }
}
