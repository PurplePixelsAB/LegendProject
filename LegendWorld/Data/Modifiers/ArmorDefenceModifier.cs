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

        public override void Update(GameTime gameTime, Character character)
        {            
            if (character.IsEquiped(this.Required))
                character.Stats.Factor(StatIdentifier.Armor, this.Amount);
        }
    }
}
