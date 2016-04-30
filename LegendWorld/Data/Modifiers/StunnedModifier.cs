using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Modifiers
{
    public class StunnedModifier : CharacterModifier
    {
        public StunnedModifier(int duration) : base()
        {
            base.Duration = duration;
            base.IsUsed = false;
        }

        internal override float ModifyMovement(float movement)
        {
            return 0f;
        }
    }
}
