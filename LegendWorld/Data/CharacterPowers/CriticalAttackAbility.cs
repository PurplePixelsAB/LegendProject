﻿using Data.World;
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
    public class CriticalAttackAbility : CharacterPower
    {
        public CriticalAttackAbility() : base(CharacterPowerIdentity.CriticalAttack)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 60;
        }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        public override bool CanBePerformedBy(Character character)
        {
            if (character.Stats.HasModifier(typeof(DoublePowerModifier)))
                return false;

            return base.CanBePerformedBy(character);
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new DoublePowerModifier());
        }

        //internal override void PerformBy(WorldState worldState, Character character)
        //{
        //    base.PerformBy(worldState, character);
        //}
    }
}
