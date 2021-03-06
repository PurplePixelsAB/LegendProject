﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;

namespace LegendWorld.Data.Modifiers
{
    public class SpeedModifier : CharacterModifier
    {
        public SpeedModifier(float amount) : base()
        {
            base.Duration = null;
            this.Amount = amount;
            base.IsUsed = false;
        }

        public float Amount { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Mobility, this.OnMobility);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.Mobility, this.OnMobility);
        }

        private void OnMobility(Character character, StatReadEventArgs e)
        {
            e.Value = Stats.Factor(e.Value, this.Amount);
        }

        //public override void Update(GameTime gameTime, Character character)
        //{
        //    character.Stats.Factor(StatIdentifier.Mobility, this.Amount);
        //}
    }
}
