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
    public class HardAttackAbility : CharacterPower
    {
        private byte basePower = 34;
        //private byte baseEnergyCost = 40;

        public HardAttackAbility() : base(CharacterPowerIdentity.HardAttack)
        {
            this.Duration = 1000;
            this.PrepareTime = 1000;
            this.EnergyCost = 20;
        }

        public byte Power
        {
            get
            {
                return basePower;
            }

            set
            {
                basePower = value;
            }
        }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new ConeCollitionArea() { Range = 20, Fov = 30 };
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            int attackersPower = abilityPerformedBy.Stats.CalculateAbilityPower(this.Power);
            int damageTaken = abilityPerformedTo.Stats.CalculateDamageTaken(attackersPower);
            abilityPerformedTo.Health -= damageTaken;
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
        }
    }
}
