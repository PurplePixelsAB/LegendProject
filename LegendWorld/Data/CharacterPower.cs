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
using Data;

namespace LegendWorld.Data
{
    public abstract class CharacterPower
    {
        private static CharacterPower[] abilities = new CharacterPower[ushort.MaxValue];
        private static void Register(CharacterPower abilityToRegister)
        {
            CharacterPower.abilities[(ushort)abilityToRegister.Id] = abilityToRegister;
        }
        public static void Load()
        {
            CharacterPower.Register(new DefaultAttackAbility());
            CharacterPower.Register(new CriticalAttackAbility());
            CharacterPower.Register(new StunAttackAbility());
            CharacterPower.Register(new SlowingAttackAbility());
            CharacterPower.Register(new DecreaseEnergyCostAbility());
            CharacterPower.Register(new IncreaseEnergyCostAbility());
            CharacterPower.Register(new DecreaseDurationAbility());
            CharacterPower.Register(new IncreaseDurationAbility());
            CharacterPower.Register(new MeditationAbility());
            CharacterPower.Register(new AbsorbDamageAbility());
            CharacterPower.Register(new DeflectDamageAbility());
            CharacterPower.Register(new ShortSpeedBurstAbility());
            CharacterPower.Register(new IncreaseSpeedAbility());
        }
        public static CharacterPower Get(CharacterPowerIdentity id)
        {
            return abilities[(ushort)id];
        }
        public static List<CharacterPower> GetAll()
        {
            return abilities.ToList();
        }

        private CharacterPowerIdentity id;

        public CharacterPower(CharacterPowerIdentity abilityIdentity)
        {
            this.id = abilityIdentity;
        }

        public CharacterPowerIdentity Id { get { return id; } }

        public ItemData.ItemIdentity? RequiredItem { get; set; }
        public byte EnergyCost { get; set; }

        public int PrepareTime { get; set; }
        public int Duration { get; set; }

        public virtual bool CanBePerformedBy(Character character)
        {
            if (!character.Powers.Contains(this.Id) && Id != CharacterPowerIdentity.DefaultAttack)
                return false;

            if (character.IsBusy)
                return false;

            if (character.IsDead)
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
            abilityPerformedBy.OnPerformsPower(this);
            var affectedCharacters = this.GetAbilityEffectArea(worldState, abilityPerformedBy).GetAffected(worldState, abilityPerformedBy);
            foreach (Character affectedChar in affectedCharacters)
            {
                this.PerformTo(worldState, affectedChar, abilityPerformedBy);
            }
        }

        internal virtual void PerformTo(WorldState worldState, Character abilityPerformedTo, Character abilityPerformedBy)
        {
            abilityPerformedTo.OnAffectedByPower(this, abilityPerformedBy);
        }
        public virtual CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new NoneColltionArea();
        }
    }
}
