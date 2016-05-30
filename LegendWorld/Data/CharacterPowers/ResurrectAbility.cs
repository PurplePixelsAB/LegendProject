using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Data;

namespace LegendWorld.Data.Abilities
{
    public class ResurrectAbility : CharacterPower
    {
        public ResurrectAbility() : base(CharacterPowerIdentity.Resurrect)
        {
            this.Duration = 1000;
            this.PrepareTime = 0;
            this.EnergyCost = 99;
        }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character performedBy)
        {
            return new CircleCollitionArea() { Position = performedBy.AimToPosition, R = 40 };
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            if (abilityPerformedTo.IsDead)
                abilityPerformedTo.Stats.Health = 10;
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
