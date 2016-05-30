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
    public class StealthAbility : CharacterPower
    {
        public StealthModifier Modifier { get; private set; }

        public StealthAbility() : base(CharacterPowerIdentity.Stealth)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 1;
            this.Modifier = new StealthModifier(.1f);
        }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            if (!abilityPerformedTo.Stats.Modifiers.Contains(this.Modifier))
                abilityPerformedTo.Stats.Modifiers.Add(this.Modifier);
            else
                abilityPerformedTo.Stats.Modifiers.Remove(this.Modifier);
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}