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

        //internal override int ModifyEnergyCost(int energy)
        //{
        //    return base.ModifyEnergyCost(energy);
        //}
        public override void Update(GameTime gameTime, Character character)
        {
            character.Stats.Modify(StatIdentifier.EnergyCost, this.Amount);
        }
    }
}
