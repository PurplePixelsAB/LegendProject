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
    public class DefaultAttackAbility : CharacterPower
    {
        private byte basePower = 5;
        //private byte baseEnergyCost = 40;

        public DefaultAttackAbility() : base(CharacterPowerIdentity.DefaultAttack)
        {
            this.Duration = 2000;
            this.PrepareTime = 0;
            this.EnergyCost = 5;
            //this.Area = new ConeCollitionArea();
            //this.Area.Range = 20;
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
            else if (performedBy.RightHand.Data.Identity == ItemData.ItemIdentity.Bow)
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
                byte attackersPower = abilityPerformedBy.Stats.CalculateAbilityPower(this.Power);
                byte damageTaken = abilityPerformedTo.Stats.CalculateDamageTaken(attackersPower);
                abilityPerformedTo.Health -= damageTaken;
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
            //if (character.RightHand != null)
            //{
            //    if (character.IsEquiped(ItemData.ItemIdentity.Bow))
            //    {
            //        worldState.ShootArrow(character, character.RightHand);
            //    }
            //}
        }

        //public int Prepare { get; private set; }
        //public ConeCollitionArea Area { get; private set; }

        //public void Perform(WorldState world, Character character)
        //{
        //    if (character.Energy >= this.EnergyCost)
        //    {
        //        var affectedCharacters = this.GetAbilityEffectArea(character.Position).GetAffected(world, character);
        //        this.Perform(world, character, affectedCharacters);
        //    }
        //}
        //private void Perform(WorldState world, Character character, IList<Character> affectedCharacters)
        //{
        //    foreach (var affectedChar in affectedCharacters)
        //    {
        //        affectedChar.Health -= BaseDamage;
        //    }
        //}
    }
}
