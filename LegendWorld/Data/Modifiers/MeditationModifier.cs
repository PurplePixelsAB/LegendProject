using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data.Modifiers
{
    public class MeditationModifier : CharacterModifier
    {
        private TimeSpan timeToMaxAmount = new TimeSpan(0, 0, 0, 10);
        private long elapstedTime = 0;
        public float StartAmount { get; private set; }
        public float EndAmount { get; private set; }

        public MeditationModifier(float startAmount, float endAmount)
        {
            this.StartAmount = startAmount;
            this.EndAmount = endAmount;
        }

        public override void Update(GameTime gameTime, Character character)
        {
            if (character.IsMoving)
                character.Stats.Modifiers.Remove(this);
            
            elapstedTime += gameTime.ElapsedGameTime.Ticks;
            float lerpAmount = elapstedTime / timeToMaxAmount.Ticks;
            float modAmount = MathHelper.Lerp(this.StartAmount, this.EndAmount, lerpAmount);

            character.Stats.Factor(StatIdentifier.EnergyRegeneration, modAmount);
        }
    }
}
