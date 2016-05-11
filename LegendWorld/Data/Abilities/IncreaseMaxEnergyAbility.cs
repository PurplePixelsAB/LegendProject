using Data.World;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Modifiers;

namespace LegendWorld.Data.Abilities
{
    public class IncreaseMaxEnergyAbility : Ability
    {
        public IncreaseMaxEnergyAbility() : base(AbilityIdentity.IncreaseEnergyCost)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 0;
            this.NewMax = 150;
        }

        public byte NewMax { get; private set; }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new SelfCollitionArea();
        }

        internal override void Update(GameTime gameTime, WorldState worldState, Character abilityOwner)
        {
            base.Update(gameTime, worldState, abilityOwner);
            if (!abilityOwner.Stats.HasModifier(typeof(MaxEnergyModifier)))
                abilityOwner.Stats.Modifiers.Add(new MaxEnergyModifier(this.NewMax));
        }
    }
}
