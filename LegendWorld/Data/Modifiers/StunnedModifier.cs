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
    public class StunnedModifier : CharacterModifier
    {
        public StunnedModifier(int duration) : base()
        {
            base.Duration = duration;
            base.IsUsed = false;
        }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Modify(StatIdentifier.MovementSpeed, 0f);
            character.BusyDuration = base.Duration.Value;
        }
    }
}
