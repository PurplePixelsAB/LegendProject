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
    public class ArmorDefenceModifier : CharacterModifier
    {
        public ArmorDefenceModifier(float amount, ItemData.ItemIdentity armorRequired) : base()
        {
            base.Duration = null;
            this.Amount = amount;
            base.IsUsed = false;
            this.Required = armorRequired;
        }

        public float Amount { get; private set; }
        public ItemData.ItemIdentity Required { get; private set; }

        internal override void Register(Stats stats)
        {
            base.Register(stats);
            stats.OnStatReadRegister(StatIdentifier.Armor, this.OnRead);
        }
        internal override void UnRegister(Stats stats)
        {
            base.UnRegister(stats);
            stats.OnStatReadUnRegister(StatIdentifier.Armor, this.OnRead);
        }
        private StatReadEventArgs OnRead(Character character, StatReadEventArgs e)
        {
            if (character.IsEquiped(this.Required))
                e.Value = Stats.Factor(e.Value, this.Amount);

                return e;
        }

        //public override void Update(GameTime gameTime, Character character)
        //{            
        //    //if (character.IsEquiped(this.Required))
        //    //    character.Stats.Factor(StatIdentifier.Armor, this.Amount);
        //}

        //public override int Modify(Character character, StatIdentifier stat, int statValue)
        //{
        //    return character.Stats.Factor(base.Modify(character, stat, statValue), this.Amount);
        //}
    }
}
