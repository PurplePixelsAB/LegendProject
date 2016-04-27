using LegendWorld.Data.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data
{
    public abstract class Ability
    {
        private AbilityIdentity id;

        public Ability(AbilityIdentity abilityIdentity)
        {
            this.id = abilityIdentity;
        }

        public AbilityIdentity Id { get { return id; } }

        public ushort RequiredItemId { get; set; }
        public byte EnergyCost { get; set; }

        public int PrepareTime { get; set; }
        public int Duration { get; set; }

        public virtual bool CanBePerformedBy(Character character)
        {
            if (character.Abilities.Contains(this.Id))
                return false;

            if (character.Energy < this.EnergyCost)
                return false;

            return true;
        }

        internal virtual void PerformBy(WorldState worldState, Character abilityPerformedBy)
        {
            abilityPerformedBy.Energy -= this.EnergyCost;
            var affectedCharacters = this.GetAbilityEffectArea().GetAffected(worldState, abilityPerformedBy);
            foreach (Character affectedChar in affectedCharacters)
            {
                this.PerformTo(worldState, affectedChar, abilityPerformedBy);
            }
        }

        protected abstract void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy);
        public abstract CollitionArea GetAbilityEffectArea();
    }
}
