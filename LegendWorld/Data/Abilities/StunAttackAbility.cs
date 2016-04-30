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
    public class StunAttackAbility : Ability
    {
        public int StunDuration { get; private set; }

        public StunAttackAbility() : base(AbilityIdentity.StunAttack)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 60;
            this.StunDuration = 10000;
        }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new ConeCollitionArea() { Range = 20, Fov = 30 };
        }

        protected override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Modifiers.Add(new StunnedModifier(this.StunDuration));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
