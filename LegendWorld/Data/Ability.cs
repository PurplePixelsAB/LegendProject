using LegendWorld.Data.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Modifiers;

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

        public ItemIdentity? RequiredItem { get; set; }
        public byte EnergyCost { get; set; }

        public int PrepareTime { get; set; }
        public int Duration { get; set; }

        public virtual bool CanBePerformedBy(Character character)
        {
            if (character.Abilities.Contains(this.Id))
                return false;

            if (character.IsBusy)
                return false;

            if (character.Energy < this.EnergyCost)
                return false;

            if (this.RequiredItem.HasValue)
            {
                if (!character.HasItemEquiped(this.RequiredItem.Value))
                    return false;
            }

            if (character.HasModifier(typeof(StunnedModifier)))
                return false;

            return true;
        }

        internal virtual void PerformBy(WorldState worldState, Character abilityPerformedBy)
        {
            //abilityPerformedBy.Performing = this;
            abilityPerformedBy.BusyDuration += this.Duration; 
            abilityPerformedBy.Energy -= this.EnergyCost;
            var affectedCharacters = this.GetAbilityEffectArea().GetAffected(worldState, abilityPerformedBy);
            foreach (Character affectedChar in affectedCharacters)
            {
                this.PerformTo(worldState, affectedChar, abilityPerformedBy);
            }
        }

        protected virtual void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {

        }
        public abstract CollitionArea GetAbilityEffectArea();
    }
}
