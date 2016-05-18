using Data.World;
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
    public class IncreaseEnergyRegenAbility : CharacterPower
    {
        public IncreaseEnergyRegenAbility() : base(CharacterPowerIdentity.IncreaseEnergyRegen)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 0;
            this.Regeneration = 2;
        }

        public byte Regeneration { get; private set; }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new SelfCollitionArea();
        }

        internal override void Update(GameTime gameTime, WorldState worldState, Character abilityOwner)
        {
            base.Update(gameTime, worldState, abilityOwner);
            if (!abilityOwner.Stats.HasModifier(typeof(EnergyRegenerationModifier)))
                abilityOwner.Stats.Modifiers.Add(new EnergyRegenerationModifier(this.Regeneration));
        }
    }
}
