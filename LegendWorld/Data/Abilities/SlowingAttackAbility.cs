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
    public class SlowingAttackAbility : Ability
    {
        public float SlowAmount { get; private set; }
        public int StunDuration { get; private set; }

        public SlowingAttackAbility() : base(AbilityIdentity.SlowingAttack)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 60;
            this.StunDuration = 20000;
            this.SlowAmount = .5f;
        }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new ConeCollitionArea() { Range = 20, Fov = 30 };
        }

        protected override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Modifiers.Add(new SlowedModifier(this.StunDuration, this.SlowAmount));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
