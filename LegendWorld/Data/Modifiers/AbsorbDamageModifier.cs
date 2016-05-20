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
    public class AbsorbDamageModifier : CharacterModifier
    {
        public AbsorbDamageModifier() : base()
        {
            base.Duration = null;
            base.IsUsed = false;
            base.EffectName = "TestEffect1";
        }

        public override void Update(GameTime gameTime, Character character)
        {
            //if (character.Energy == 0) //No need to remove, will just not absorb any damage if there is no Energy.
            //    character.Stats.Modifiers.Remove(this);
        }

        public override byte Modify(Character character, StatIdentifier stat, byte newValue, byte oldValue)
        {
            if (stat == StatIdentifier.Health && newValue < oldValue)
            {
                byte damageToAbsorb = 0;
                int maxAbsorbAmount = character.Energy;
                int damageAmount = oldValue - newValue;
                if (damageAmount < maxAbsorbAmount)
                {
                    damageToAbsorb = (byte)MathHelper.Clamp(damageAmount, byte.MinValue, byte.MaxValue);
                    character.Energy -= damageToAbsorb;
                    return oldValue;
                }

                damageToAbsorb = (byte)MathHelper.Clamp(maxAbsorbAmount, byte.MinValue, byte.MaxValue);
                character.Energy -= damageToAbsorb;

                byte returnHealth = (byte)MathHelper.Clamp(newValue + damageToAbsorb, byte.MinValue, byte.MaxValue);
                return returnHealth;
            }
            else
                return base.Modify(character, stat, newValue, oldValue);
        }
    }
}
