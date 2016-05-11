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
    public class MaxHealthModifier : CharacterModifier
    {
        public MaxHealthModifier(byte setTo) : base()
        {
            base.Duration = null;
            this.NewMaxHealth = setTo;
            base.IsUsed = false;
        }

        public byte NewMaxHealth { get; private set; }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Modify(StatIdentifier.HealthMax, this.NewMaxHealth);
        }
    }
}
