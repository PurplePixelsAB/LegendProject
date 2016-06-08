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
    public class IncreaseSwordPowerAbility : CharacterPower
    {
        public IncreaseSwordPowerAbility() : base(CharacterPowerIdentity.IncreaseSwordPower)
        {
            this.Duration = 0;
            this.PrepareTime = 0;
            this.EnergyCost = 0;
            this.RequiredItem = ItemIdentity.Sword;
            this.Amount = 2f;
        }

        public float Amount { get; private set; }

        public override CollitionArea GetAbilityEffectArea(WorldState worldState, Character abilityPerformedBy)
        {
            return new SelfCollitionArea();
        }

        internal override void Update(GameTime gameTime, WorldState worldState, Character abilityOwner)
        {
            base.Update(gameTime, worldState, abilityOwner);
            if (!abilityOwner.Stats.HasModifier(typeof(WeaponPowerModifier)))
                abilityOwner.Stats.Modifiers.Add(new WeaponPowerModifier(this.Amount, this.RequiredItem.Value));
        }
    }
}