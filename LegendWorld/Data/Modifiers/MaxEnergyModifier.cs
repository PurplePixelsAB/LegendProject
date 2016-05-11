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
    public class MaxEnergyModifier : CharacterModifier
    {
        public MaxEnergyModifier(byte setTo) : base()
        {
            base.Duration = null;
            this.NewMax = setTo;
            base.IsUsed = false;
        }

        public byte NewMax { get; private set; }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Modify(StatIdentifier.EnergyMax, this.NewMax);
        }
    }
}
