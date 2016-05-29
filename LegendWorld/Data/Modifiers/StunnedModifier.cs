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
    public class StunnedModifier : CharacterModifier
    {
        public StunnedModifier(int duration) : base()
        {
            base.Duration = duration;
            base.IsUsed = false;
        }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Mobility, this.OnMobility);
        }

        private StatReadEventArgs OnMobility(Character character, StatReadEventArgs e)
        {
            e.Value = 0;
            return e;
        }


        public override void Update(GameTime gameTime, Character character)
        {
            //character.Stats.Factor(StatIdentifier.Mobility, 0f);
            character.BusyDuration = base.Duration.Value;
        }
    }
}
