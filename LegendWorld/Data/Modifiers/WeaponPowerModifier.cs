using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.World;
using Network;
using Data;

namespace LegendWorld.Data.Modifiers
{
    public class WeaponPowerModifier : CharacterModifier
    {
        public WeaponPowerModifier(float amount, ItemData.ItemIdentity weaponRequired) : base()
        {
            base.Duration = null;
            this.Amount = amount;
            base.IsUsed = false;
            this.WeaponRequired = weaponRequired;
        }

        public float Amount { get; private set; }
        public ItemData.ItemIdentity WeaponRequired { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Power, this.OnWeaponPower);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.Power, this.OnWeaponPower);
        }

        private StatReadEventArgs OnWeaponPower(Character character, StatReadEventArgs e)
        {
            e.Value = Stats.Factor(e.Value, this.Amount);
            return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{            
        //    if (character.IsEquiped(this.WeaponRequired))
        //        character.Stats.Factor(StatIdentifier.Power, this.Amount);
        //}
    }
}
