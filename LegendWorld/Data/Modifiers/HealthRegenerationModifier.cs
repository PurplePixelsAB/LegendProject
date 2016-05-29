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
    public class HealthRegenerationModifier : CharacterModifier
    {
        public HealthRegenerationModifier(int addTo) : base()
        {
            base.Duration = null;
            this.Regeneration = addTo;
            base.IsUsed = false;
        }

        public int Regeneration { get; private set; }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    character.Stats.Set(StatIdentifier.HealthRegeneration, this.HealthRegeneration);
        //}

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.HealthRegeneration, this.OnReadRegeneration);
        }

        private StatReadEventArgs OnReadRegeneration(Character character, StatReadEventArgs e)
        {
            e.Value += this.Regeneration; // character.Stats.Factor(e.Value, this.Amount);
            return e;
        }
    }
}
