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

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Visibility, this.OnGetVisibility);
        }

        private StatReadEventArgs OnGetVisibility(Character character, StatReadEventArgs e)
        {
            e.Value = Stats.Factor(e.Value, this.Amount);
            return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    //character.Stats.Factor(StatIdentifier.Visibility, this.Amount);
        //}
    }

    public class StealthModifier : VisibilityModifier
    {
        public StealthModifier(float amount) : base(amount)
        {
            this.RegenCost = -2;
        }

        public int RegenCost { get; set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.EnergyRegeneration, this.ModifyEnergyRegen);
        }

        private StatReadEventArgs ModifyEnergyRegen(Character character, StatReadEventArgs e)
        {
            e.Value += this.RegenCost;
            return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    //character.Stats.Factor(StatIdentifier.EnergyRegeneration, 0f);
        //    //base.Update(gameTime, character);
        //}

        //public override int Modify(Character character, StatIdentifier stat, int statValue)
        //{
        //    if (stat == StatIdentifier.EnergyRegeneration)
        //        return base.Modify(character, stat, statValue - this.RegenCost);
        //    else
        //        return base.Modify(character, stat, statValue);
        //}
    }
}
