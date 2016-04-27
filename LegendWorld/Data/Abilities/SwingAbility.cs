using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Abilities
{
    public class SwingAbilityEffect
    {
        private byte baseDamage = 5;
        private byte baseEnergyCost = 40;

        public SwingAbilityEffect()
        {
            this.Duration = 2000;
            this.Prepare = 0;
            this.Area = new ConeCollitionArea();
            this.Area.Range = 20;
        }

        public int Duration { get; set; }
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

        public byte BaseEnergyCost
        {
            get
            {
                return baseEnergyCost;
            }

            set
            {
                baseEnergyCost = value;
            }
        }

        public int Prepare { get; private set; }
        public ConeCollitionArea Area { get; private set; }

        public void Perform(WorldState world, Character character)
        {
            if (character.Energy >= this.BaseEnergyCost)
            {
                var affectedCharacters = this.Area.GetAffected(world, character);
                this.Perform(world, character, affectedCharacters);
            }
        }
        private void Perform(WorldState world, Character character, IList<Character> affectedCharacters)
        {
            foreach (var affectedChar in affectedCharacters)
            {
                affectedChar.Health -= BaseDamage;
            }

            character.Energy -= BaseEnergyCost;
        }
    }
}
