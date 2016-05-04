using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;

namespace LegendWorld.Data.Modifiers
{
    public class DoublePowerModifier : CharacterModifier
    {
        public DoublePowerModifier() : base()
        {
            base.Duration = null;
            base.IsUsed = false; //ToDo: Need to detect if chracter does damage. Maybe hook an event from Character?
        }

        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Modify(StatIdentifier.Power, 2f);
        }

        //internal override int ModifyPower(int power)
        //{
        //    base.IsUsed = true;
        //    return power;
        //}
    }
}
