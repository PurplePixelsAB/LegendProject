using System;

namespace LegendWorld.Data.Modifiers
{
    public abstract class CharacterModifier
    {
        public double? Duration { get; internal set; }
        public bool IsUsed { get; protected set; }

        internal virtual int ModifyPower(int power)
        {
            return power;
        }

        internal virtual float ModifyMovement(float movement)
        {
            return movement;
        }
    }
}