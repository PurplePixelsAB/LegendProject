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
    public class IncreaseMaxHealthAbility : Ability
    {
        public IncreaseMaxHealthAbility() : base(AbilityIdentity.IncreaseEnergyCost)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 0;
            this.NewMaxHealth = 130;
        }

        public byte NewMaxHealth { get; private set; }

        public override CollitionArea GetAbilityEffectArea()
        {
            return new SelfCollitionArea();
        }

        internal override void Update(GameTime gameTime, WorldState worldState, Character abilityOwner)
        {
            base.Update(gameTime, worldState, abilityOwner);
            if (!abilityOwner.Stats.HasModifier(typeof(MaxHealthModifier)))
                abilityOwner.Stats.Modifiers.Add(new MaxHealthModifier(this.NewMaxHealth));
        }
    }
}
