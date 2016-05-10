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
    public class DeflectDamageModifier : CharacterModifier
    {
        public DeflectDamageModifier() : base() //ToDo: Requires Shield?
        {
            base.Duration = null;
            base.IsUsed = false;
        }

        public override void Update(GameTime gameTime, Character character)
        {

        }

        public override byte Modify(Character character, StatIdentifier stat, byte newValue, byte oldValue)
        {
            if (stat == StatIdentifier.Health && newValue < oldValue && !this.IsUsed)
            {
                this.IsUsed = true;
                return oldValue;
            }
            else
                return base.Modify(character, stat, newValue, oldValue);
        }
    }
}
