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
    public class SlowingAttackAbility : CharacterPower
    {
        public float SlowAmount { get; private set; }
        public int StunDuration { get; private set; }

        public SlowingAttackAbility() : base(CharacterPowerIdentity.SlowingAttack)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 60;
            this.StunDuration = 20000;
            this.SlowAmount = .5f;
        }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new ConeCollitionArea() { Range = 20, Fov = 30 };
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Stats.Modifiers.Add(new TimedSpeedModifier(this.StunDuration, this.SlowAmount));
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
