using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;

namespace LegendWorld.Data.Modifiers
{
    public class EnergyRegenerationModifier : CharacterModifier
    {
        public EnergyRegenerationModifier(int addTo) : base()
        {
            base.Duration = null;
            this.Regeneration = addTo;
            base.IsUsed = false;
        }

        public int Regeneration { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.EnergyRegeneration, this.OnReadEnergyRegeneration);
        }

        private StatReadEventArgs OnReadEnergyRegeneration(Character character, StatReadEventArgs e)
        {
            e.Value += this.Regeneration; // character.Stats.Factor(e.Value, this.Amount);
            return e;
        }

        //private StatReadEventArgs OnReadPower(Character character, StatReadEventArgs e)
        //{
        //    e.Value = character.Stats.Factor(e.Value, this.Amount);
        //    return e;
        //}

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    character.Stats.Set(StatIdentifier.EnergyRegeneration, this.Regeneration);
        //}
    }
}
