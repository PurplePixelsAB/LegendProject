using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Modifiers;
using Data;

namespace LegendWorld.Data.Abilities
{
    public class MeditationAbility : CharacterPower
    {
        public MeditationAbility() : base(CharacterPowerIdentity.Meditation)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 1;
            this.StartValue = 1.1f;
            this.EndValue = 5f;
        }

        public float EndValue { get; private set; }
        public float StartValue { get; private set; }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new MeditationModifier(this.StartValue, this.EndValue));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            if (!character.Stats.HasModifier(typeof(MeditationModifier)))
                base.PerformBy(worldState, character);
        }
    }
}