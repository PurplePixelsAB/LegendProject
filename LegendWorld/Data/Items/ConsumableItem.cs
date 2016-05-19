﻿using System;
using Data.World;
using System.Runtime.Serialization;
using Network;

namespace LegendWorld.Data.Items
{
    public abstract class ConsumableItem : StackableItem
    {
        public ConsumableItem()
        {
            this.Category = ItemCategory.Consumable;
        }
        public abstract bool OnUse(Character usedBy, WorldState worldState);

        public bool Use(Character character, WorldState worldState)
        {
            if (character == null)
                return false;
            if (character.IsDead)
                return false;
            if (character.IsBusy)
                return false;

            bool result = this.OnUse(character, worldState);
            if (result)
            {
                this.StackCount--;
                if (this.StackCount <= 0)
                    worldState.RemoveItem(this);
            }

            return result;
        }
    }
}