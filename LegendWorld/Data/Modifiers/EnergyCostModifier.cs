using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data.Modifiers
{
    public class EnergyCostModifier : CharacterModifier
    {
        public EnergyCostModifier(float amount)
        {
            this.Amount = amount;
        }

        public float Amount { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Power, this.OnReadPower);
            stats.OnStatReadRegister(StatIdentifier.EnergyCost, this.OnReadEnergyCost);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.Power, this.OnReadPower);
            stats.OnStatReadUnRegister(StatIdentifier.EnergyCost, this.OnReadEnergyCost);
        }

        private StatReadEventArgs OnReadEnergyCost(Character character, StatReadEventArgs e)
        {
            e.Value = Stats.Factor(e.Value, this.Amount);
            return e;
        }

        private StatReadEventArgs OnReadPower(Character character, StatReadEventArgs e)
        {
            e.Value = Stats.Factor(e.Value, this.Amount);
            return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    character.Stats.Factor(StatIdentifier.EnergyCost, this.Amount);
        //    character.Stats.Factor(StatIdentifier.Power, this.Amount);
        //}
    }
}
