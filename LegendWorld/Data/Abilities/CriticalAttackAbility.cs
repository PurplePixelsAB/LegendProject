using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Modifiers;

namespace LegendWorld.Data.Abilities
{
    public class CriticalAttackAbility : Ability
    {
        public CriticalAttackAbility() : base(AbilityIdentity.CriticalAttack)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 60;
        }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new SelfCollitionArea();
        }

        public override bool CanBePerformedBy(Character character)
        {
            if (character.Stats.HasModifier(typeof(DoublePowerModifier)))
                return false;

            return base.CanBePerformedBy(character);
        }

        protected override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new DoublePowerModifier());
        }

        //internal override void PerformBy(WorldState worldState, Character character)
        //{
        //    base.PerformBy(worldState, character);
        //}
    }
}
