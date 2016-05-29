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
    public class MaxHealthModifier : CharacterModifier
    {
        public MaxHealthModifier(int addedMax) : base()
        {
            base.Duration = null;
            this.AddedMax = addedMax;
            base.IsUsed = false;
        }

        public int AddedMax { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.HealthMax, this.OnMaxHealth);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.HealthMax, this.OnMaxHealth);
        }

        private StatReadEventArgs OnMaxHealth(Character character, StatReadEventArgs e)
        {
            e.Value += this.AddedMax;
            return e;
        }
        //public override void Update(GameTime gameTime, Character character)
        //{
        //    character.Stats.Set(StatIdentifier.HealthMax, this.NewMaxHealth);
        //}
    }
}
