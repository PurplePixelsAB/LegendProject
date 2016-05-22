using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;

namespace LegendWorld.Data.Modifiers
{
    public class TimedSpeedModifier : CharacterModifier
    {
        public TimedSpeedModifier(int duration, float amount) : base()
        {
            base.Duration = duration;
            this.Amount = amount;
            base.IsUsed = false;
        }

        public float Amount { get; private set; }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Factor(StatIdentifier.MovementSpeed, this.Amount);
        }
    }
}
