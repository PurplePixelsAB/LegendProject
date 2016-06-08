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
        private byte basePower = 24;
        //private byte baseEnergyCost = 40;

        public HardAttackAbility() : base(CharacterPowerIdentity.HardAttack)
        {
            this.Duration = 1000;
            this.PrepareTime = 1000;
            this.EnergyCost = 15;
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

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character performedBy)
        {
            if (performedBy.RightHand == null && performedBy.LeftHand == null)
                return new ConeCollitionArea() { Range = 20, Fov = 90 };
            else if (performedBy.RightHand.Identity == ItemIdentity.Bow)
            {
                ArrowColltionArea arrowColltionArea = new ArrowColltionArea(this, performedBy);
                worldState.Projectiles.Add(arrowColltionArea);
                return arrowColltionArea;
            }
            else
                return new ConeCollitionArea() { Range = performedBy.RightHand.SwingRange, Fov = performedBy.RightHand.SwingFov };
        }

        internal override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            int attackersPower = abilityPerformedBy.Stats.GetStat(StatIdentifier.Power); //abilityPerformedBy.Stats.CalculateAbilityPower(this.Power + weaponPower);
            int damageTaken = abilityPerformedTo.Stats.CalculateDamageTaken(attackersPower);
            abilityPerformedTo.Stats.Health -= damageTaken;
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            int weaponPower = character.Stats.GetWeaponPower();
            character.Stats.SetPower(this.Power, weaponPower);
            base.PerformBy(worldState, character);
        }
    }
}
