using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data.World;

namespace LegendWorld.Data.Modifiers
{
    public class VisibilityModifier : CharacterModifier
    {
        public VisibilityModifier(float amount) : base()
        {
            base.Duration = null;
            base.IsUsed = false;
            this.Amount = amount;
        }

        public float Amount { get; set; }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Factor(StatIdentifier.Visibility, this.Amount);
        }
    }

    public class StealthModifier : VisibilityModifier
    {
        public StealthModifier(float amount) : base(amount)
        {
        }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Factor(StatIdentifier.EnergyRegeneration, 0f);
            base.Update(gameTime, character);
        }
    }
}
