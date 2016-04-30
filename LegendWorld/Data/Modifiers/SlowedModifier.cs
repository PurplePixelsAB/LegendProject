using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Modifiers
{
    public class SlowedModifier : CharacterModifier
    {
        public SlowedModifier(int duration, float amount) : base()
        {
            base.Duration = duration;
            this.Amount = amount;
            base.IsUsed = false;
        }

        public float Amount { get; private set; }

        internal override float ModifyMovement(float movement)
        {
            return movement * this.Amount;
        }
    }
}
