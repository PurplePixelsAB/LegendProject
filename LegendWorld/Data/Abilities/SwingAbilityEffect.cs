using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data.Abilities
{
    public class SwingAbility : Ability
    {
        private byte baseDamage = 5;
        private byte baseEnergyCost = 40;

        public SwingAbility() : base(AbilityIdentity.Swing)
        {
            this.Duration = 2000;
            this.Prepare = 0;
            //this.Area = new ConeCollitionArea();
            //this.Area.Range = 20;
        }

        public byte BaseDamage
        {
            get
            {
                return baseDamage;
            }

            set
            {
                baseDamage = value;
            }
        }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new ConeCollitionArea() { Range = 20 };
        }

        protected override void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.Health -= BaseDamage;
        }

        internal override void PerformBy(WorldState worldState, Character character)
        {
            base.PerformBy(worldState, character);
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
