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
    public class AbsorbDamageAbility : CharacterPower
    {
        public AbsorbDamageAbility() : base(CharacterPowerIdentity.AbsorbDamage) //ToDo: Requires Shield?
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 0;
            this.Enabled = false;
            this.Modifier = new AbsorbDamageModifier();
        }

        public bool Enabled { get; private set; }
        public AbsorbDamageModifier Modifier { get; private set; }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            if (!this.Enabled)
            {
                this.Enabled = true;
                if (!abilityPerformedTo.Stats.Modifiers.Contains(this.Modifier))
                    abilityPerformedTo.Stats.Modifiers.Add(this.Modifier);
            }
            else
            {
                this.Enabled = false;
                if (abilityPerformedTo.Stats.Modifiers.Contains(this.Modifier))
                    abilityPerformedTo.Stats.Modifiers.Remove(this.Modifier);
            }
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}