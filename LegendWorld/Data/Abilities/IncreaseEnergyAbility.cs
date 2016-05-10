﻿using Data.World;
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
    public class IncreaseEnergyCostAbility : Ability
    {
        public IncreaseEnergyCostAbility() : base(AbilityIdentity.IncreaseEnergyCost)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 1;
            this.EnergyModifier = 1.5f;
        }

        public float EnergyModifier { get; private set; }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new SelfCollitionArea();
        }

        protected override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new EnergyCostModifier(this.EnergyModifier));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
