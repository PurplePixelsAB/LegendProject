using Data.World;
using Microsoft.Xna.Framework;
using Network;
using System;

namespace LegendWorld.Data.Modifiers
{
    public abstract class CharacterModifier
    {
        public double? Duration { get; internal set; }
        public bool IsUsed { get; protected set; }

        public string EffectName { get; protected set; }

        public abstract void Update(GameTime gameTime, Character character);
        
        public virtual int Modify(Character character, StatIdentifier stat, int newValue, int oldValue)
        {
            return newValue;
        }

        //internal virtual float ModifyPower(float powerModifier)
        //{
        //    return powerModifier;
        //}

        //internal virtual float ModifySpeed(float speedModifier)
        //{
        //    return speedModifier;
        //}

        //internal virtual float ModifyEnergyCost(float energyCostModifier)
        //{
        //    return energyCostModifier;
        //}

        //internal virtual float ModifyMaxEnergy(float maxEnergyModifier)
        //{
        //    return maxEnergyModifier;
        //}

        //internal virtual float ModifyMaxHealth(float maxHealthModifier)
        //{
        //    return maxHealthModifier;
        //}
    }
}