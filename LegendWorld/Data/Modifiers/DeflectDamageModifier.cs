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

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatChangedRegister(StatIdentifier.Health, this.ReflectDamage);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatChangedRegister(StatIdentifier.Health, this.ReflectDamage);
        }

        private void ReflectDamage(Character character, StatChangedEventArgs e)
        {
            if (e.Value < e.PreviousValue && !this.IsUsed)
            {
                this.IsUsed = true;
                e.Value = e.PreviousValue;
                character.Stats.Modifiers.Remove(this);
            }            
        }

        //public override void Update(GameTime gameTime, Character character)
        //{

        //}

        //public override int Modify(Character character, StatIdentifier stat, int newValue, int oldValue)
        //{
        //    if (stat == StatIdentifier.Health && newValue < oldValue && !this.IsUsed)
        //    {
        //        this.IsUsed = true;
        //        return oldValue;
        //    }
        //    else
        //        return base.Modify(character, stat, newValue, oldValue);
        //}
    }
}
