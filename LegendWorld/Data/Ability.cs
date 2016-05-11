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
        private static Ability[] abilities = new Ability[ushort.MaxValue];
        private static void Register(Ability abilityToRegister)
        {
            Ability.abilities[(ushort)abilityToRegister.Id] = abilityToRegister;
        }
        public static void Load()
        {
            Ability.Register(new DefaultAttackAbility());
            Ability.Register(new CriticalAttackAbility());
            Ability.Register(new StunAttackAbility());
            Ability.Register(new SlowingAttackAbility());
            Ability.Register(new DecreaseEnergyCostAbility());
            Ability.Register(new IncreaseEnergyCostAbility());
            Ability.Register(new DecreaseDurationAbility());
            Ability.Register(new IncreaseDurationAbility());
            Ability.Register(new MeditationAbility());
            Ability.Register(new AbsorbDamageAbility());
            Ability.Register(new DeflectDamageAbility());
            Ability.Register(new ShortSpeedBurstAbility());
            Ability.Register(new IncreaseSpeedAbility());
        }
        public static Ability Get(AbilityIdentity id)
        {
            return abilities[(ushort)id];
        }
        public static List<Ability> GetAll()
        {
            return abilities.ToList();
        }

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
            if (!character.Abilities.Contains(this.Id))
                return false;

            if (character.IsBusy)
                return false;

            if (character.Energy < character.Stats.CalculateEnergyCost(this.EnergyCost))
                return false;

            if (this.RequiredItem.HasValue)
            {
                if (!character.HasItemEquiped(this.RequiredItem.Value))
                    return false;
            }

            if (character.Stats.HasModifier(typeof(StunnedModifier)))
                return false;

            return true;
        }

        internal virtual void Update(GameTime gameTime, WorldState worldState, Character abilityOwner)
        {

        }

        internal virtual void PerformBy(WorldState worldState, Character abilityPerformedBy)
        {
            //abilityPerformedBy.Performing = this;
            abilityPerformedBy.BusyDuration += this.Duration; 
            abilityPerformedBy.Energy -= abilityPerformedBy.Stats.CalculateEnergyCost(this.EnergyCost);
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
