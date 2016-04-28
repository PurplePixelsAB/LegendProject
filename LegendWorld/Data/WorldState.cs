using Data.World;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Network
{
    public abstract class WorldState
    {
        protected byte swingDmg = 24;
        protected byte swingEnergy = 34;
        protected byte healAmount = 11;

        protected TimeSpan baseRegenTick = new TimeSpan(0, 0, 1);
        //protected TimeSpan baseEnergyTick = new TimeSpan(0, 0, 1);
        private long nextRegendTick = 0;

        protected Dictionary<ushort, Item> items = new Dictionary<ushort, Item>();
        public Dictionary<ushort, Item>.KeyCollection Items { get { return items.Keys; } }
        public virtual Item GetItem(ushort id)
        {
            if (items.ContainsKey(id))
            {
                return items[id];
            }

            return null;
        }
        public virtual void AddItem(Item item)
        {
            if (items.ContainsKey(item.Id))
                return;

            items.Add(item.Id, item);
        }
        public virtual void RemoveItem(Item item)
        {
            if (!items.ContainsKey(item.Id))
                return;

            items.Remove(item.Id);
        }

        protected Dictionary<ushort, GroundItem> groundItems = new Dictionary<ushort, GroundItem>();
        public Dictionary<ushort, GroundItem>.KeyCollection GroundItems { get { return groundItems.Keys; } }
        public virtual GroundItem GetGroundItem(ushort id)
        {
            if (groundItems.ContainsKey(id))
            {
                return groundItems[id];
            }

            return null;
        }


        protected Dictionary<AbilityIdentity, Ability> abilities = new Dictionary<AbilityIdentity, Ability>();
        public Ability GetAbility(AbilityIdentity abilityId)
        {
            return abilities[abilityId];
        }

        protected Dictionary<ushort, Character> characters = new Dictionary<ushort, Character>();

        public virtual void AddGroundItem(GroundItem groundItem)
        {
            if (groundItems.ContainsKey(groundItem.Id))
                return;

            groundItems.Add(groundItem.Id, groundItem);
        }
        public virtual void RemoveGroundItem(GroundItem groundItem)
        {
            if (groundItems.ContainsKey(groundItem.Id))
                return;

            groundItems.Remove(groundItem.Id);
        }

        public Dictionary<ushort, Character>.KeyCollection Characters { get { return characters.Keys; } }
        public virtual Character GetCharacter(ushort id)
        {
            if (characters.ContainsKey(id))
            {
                return characters[id];
            }

            return null;
        }

        public virtual void AddCharacter(Character newCharacter)
        {
            if (characters.ContainsKey(newCharacter.Id))
                return;

            newCharacter.MoveToMapPointValidating += Character_MoveToMapPointValidating;
            characters.Add(newCharacter.Id, newCharacter);
        }
        public virtual void RemoveCharacter(Character charToRemove)
        {
            if (!characters.ContainsKey(charToRemove.Id))
                return;

            characters.Remove(charToRemove.Id);
        }

        private void Character_MoveToMapPointValidating(object sender, Character.MoveToMapPointValidatingEventArgs e)
        {
            WorldMap map = this.GetCharactersMap((Character)sender);
            e.IsValid = map.Bounds.Contains(e.MoveToMapPoint);
        }

        public virtual void Update(GameTime gameTime)
        {
            var idList = characters.Keys;

            bool isRegenTick = gameTime.TotalGameTime.Ticks >= nextRegendTick;

            foreach (ushort characterId in idList)
            {
                characters[characterId].Update(gameTime);

                if (isRegenTick)
                {
                    if (characters[characterId].Health < characters[characterId].MaxHealth - 10 && characters[characterId].Health >= 10)
                        characters[characterId].Health += 1;

                    if (characters[characterId].Energy < characters[characterId].MaxEnergy)
                        characters[characterId].Energy += 1;

                    nextRegendTick = (gameTime.TotalGameTime + baseRegenTick).Ticks;
                }
            }
        }

        public virtual bool PerformAbility(Ability ability, Character character)
        {
            if (!ability.CanBePerformedBy(character))
                return false;

            ability.PerformBy(this, character);
            return true;
        }
        public virtual bool PerformAbility(AbilityIdentity abilityId, Character character)
        {
            if (!this.abilities.ContainsKey(abilityId))
                return false;

            return this.PerformAbility(this.GetAbility(abilityId), character);
        }

        protected abstract WorldMap GetCharactersMap(Character character);


        public double VectorToRadian(Vector2 direction)
        {
            direction.Normalize();
            return Math.Atan2(direction.X, -direction.Y) + MathHelper.Pi;
        }
        //public double VectorToAngle(Vector2 vector)
        //{
        //    Vector2 direction = Vector2.Normalize(vector);
        //    return Math.Atan2(direction.Y, direction.X);
        //}

    }
}