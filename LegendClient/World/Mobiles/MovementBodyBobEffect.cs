using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.World.Mobiles
{    public class MovementBodyBobEffect
    {
        //float lerp = 0f;
        float lerpSpeed = .01f;

        public Vector2 PositioinBob { get; set; }
        public Vector2 BobValue { get; set; }

        public MovementBodyBobEffect()
        {
            BobValue = new Vector2(0f, 2f);
            PositioinBob = new Vector2(0f, 0f);
        }


        public void Update(GameTime gameTime)
        {
            double msElapsed = gameTime.TotalGameTime.Milliseconds;
            var offset = (float)Math.Sin(msElapsed * lerpSpeed);

            this.PositioinBob = Vector2.Lerp(this.BobValue, this.BobValue * -1f, offset);
        }
    }
}
