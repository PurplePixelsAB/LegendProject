using Data.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Modifiers
{
    public class RevealingActionModifier : CharacterModifier
    {
        private double totalDuration = 0D;
        public RevealingActionModifier(int duration) : base()
        {
            totalDuration = duration;
            base.Duration = duration;
            base.IsUsed = false;
        }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Visibility, this.OnGetVisibility);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.Visibility, this.OnGetVisibility);
        }

        private void OnGetVisibility(Character character, StatReadEventArgs e)
        {
            double lerpAmount = Duration.Value / totalDuration;
            float lerpValue = MathHelper.Lerp(e.Value, character.GetBaseVisibility(), (float)lerpAmount);
            int newValue = (int)Math.Round(lerpValue);

            e.Value = newValue;
        }
    }
}
