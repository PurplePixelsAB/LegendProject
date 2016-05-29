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
        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatChangedRegister(StatIdentifier.Health, this.OnHealthChanged);
        }

        private StatChangedEventArgs OnHealthChanged(Character character, StatChangedEventArgs e)
        {
            int damageToAbsorb = 0;
            int maxAbsorbAmount = character.Stats.Energy;
            int damageAmount = e.PreviousValue - e.Value;
            if (damageAmount < maxAbsorbAmount)
            {
                damageToAbsorb = MathHelper.Clamp(damageAmount, byte.MinValue, byte.MaxValue);
                character.Stats.Energy -= damageToAbsorb;
                e.Value = e.PreviousValue;
                return e;
            }

            damageToAbsorb = MathHelper.Clamp(maxAbsorbAmount, byte.MinValue, byte.MaxValue);
            character.Stats.Energy -= damageToAbsorb;

            int returnHealth = MathHelper.Clamp(e.Value + damageToAbsorb, byte.MinValue, byte.MaxValue);
            e.Value = returnHealth;
            return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    //if (character.Energy == 0) //No need to remove, will just not absorb any damage if there is no Energy.
        //    //    character.Stats.Modifiers.Remove(this);
        //}

        //public override int Modify(Character character, StatIdentifier stat, int newValue, int oldValue)
        //{
        //    if (stat == StatIdentifier.Health && newValue < oldValue)
        //    {
        //        int damageToAbsorb = 0;
        //        int maxAbsorbAmount = character.Energy;
        //        int damageAmount = oldValue - newValue;
        //        if (damageAmount < maxAbsorbAmount)
        //        {
        //            damageToAbsorb = MathHelper.Clamp(damageAmount, byte.MinValue, byte.MaxValue);
        //            character.Energy -= damageToAbsorb;
        //            return oldValue;
        //        }

        //        damageToAbsorb = MathHelper.Clamp(maxAbsorbAmount, byte.MinValue, byte.MaxValue);
        //        character.Energy -= damageToAbsorb;

        //        int returnHealth = MathHelper.Clamp(newValue + damageToAbsorb, byte.MinValue, byte.MaxValue);
        //        return returnHealth;
        //    }
        //    else
        //        return base.Modify(character, stat, newValue, oldValue);
        //}
    }
}
