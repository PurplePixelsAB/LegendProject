using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Modifiers
{
    public class DoublePowerModifier : CharacterModifier
    {
        public DoublePowerModifier() : base()
        {
            base.Duration = null;
            base.IsUsed = false;
        }

        internal override int ModifyPower(int power)
        {
            base.IsUsed = true;
            return power;
        }
    }
}
