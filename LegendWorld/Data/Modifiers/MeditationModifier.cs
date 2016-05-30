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

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.EnergyRegeneration, this.GetEnergyRegen);
        }

        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.EnergyRegeneration, this.GetEnergyRegen);
        }

        private void GetEnergyRegen(Character character, StatReadEventArgs e)
        {
            float lerpAmount = elapstedTime / timeToMaxAmount.Ticks;
            float modAmount = MathHelper.Lerp(this.StartAmount, this.EndAmount, lerpAmount);

            int energyRegen = character.Stats.EnergyRegen;
            int modER = Stats.Factor(energyRegen, modAmount);
            e.Value = modER;
            //character.Stats.Factor(StatIdentifier.EnergyRegeneration, modAmount);
        }

        public override void Update(GameTime gameTime, Character character)
        {
            if (character.IsMoving)
                character.Stats.Modifiers.Remove(this);

            elapstedTime += gameTime.ElapsedGameTime.Ticks;
        }
    }
}
