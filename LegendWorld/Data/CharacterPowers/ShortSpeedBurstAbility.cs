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
    public class ShortSpeedBurstAbility : CharacterPower
    {
        public ShortSpeedBurstAbility() : base(CharacterPowerIdentity.ShortSpeedBurst)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 20;
        }
        
        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new TimedSpeedModifier(10000, 1.5f));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}