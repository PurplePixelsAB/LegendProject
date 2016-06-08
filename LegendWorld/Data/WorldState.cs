using Data;
using Data.World;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static Data.ItemModel;

namespace Network
{
    public abstract class WorldState
    {
        //protected byte swingDmg = 24;
        //protected byte swingEnergy = 34;
        //protected byte healAmount = 11;

        protected TimeSpan baseRegenTick = new TimeSpan(0, 0, 3);
        //protected TimeSpan baseEnergyTick = new TimeSpan(0, 0, 1);
        private long nextRegendTick = 0;

        protected Dictionary<int, Item> items = new Dictionary<int, Item>();
        public Dictionary<int,Item>.KeyCollection Items { get { return items.Keys; } }

        public WorldState()
        {
            CharacterPower.Load();

            this.Projectiles = new List<ArrowColltionArea>(30);
        }

        public virtual Item GetItem(int id)
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

            if (item.ContainerId.HasValue)
            {
                ContainerItem container = (ContainerItem)this.GetItem(item.ContainerId.Value);
                if (container != null)
                {
                    if (!container.Items.Contains(item))
                        container.Items.Add(item);
                }
            }

            items.Add(item.Id, item);
        }
        public virtual void RemoveItem(Item item)
        {
            if (!items.ContainsKey((ushort)item.Id))
                return;

            items.Remove(item.Id);
            if (item.ContainerId.HasValue)
            {
                ContainerItem container = (ContainerItem)this.GetItem(item.ContainerId.Value);
                if (container != null)
                {
                    if (container.Items.Contains(item))
                        container.Items.Remove(item);
                }
            }
            item.Remove();

            //int groundItemIdToRemove = -1;
            //foreach (var groundItem in groundItems.Values)
            //{
            //    if (groundItem.ItemId == item.Id)
            //    {
            //        groundItemIdToRemove = groundItem.Id;
            //    }
            //}
            //if (groundItemIdToRemove > 0)
            //    groundItems.Remove(groundItemIdToRemove);

        }

        //internal void ShootArrow(Character character, WeaponItem rightHand)
        //{
        //    ArrowProjectile projectile = new ArrowProjectile();
        //    projectile.Position = character.Position;
        //    projectile.Target = character.AimToPosition;
        //    this.Projectiles.Add(projectile);
        //}

        public Item CreateItem(ItemModel itemData)
        {
            IItemFactory factory = this.GetItemFactory((ItemIdentity)itemData.Identity);
            Item newReturnItem = factory.CreateNew(itemData);

            return newReturnItem;
        }

        protected abstract IItemFactory GetItemFactory(ItemIdentity identity);

        //protected Dictionary<int, GroundItem> groundItems = new Dictionary<int, GroundItem>();
        //public Dictionary<int, GroundItem>.KeyCollection GroundItems { get { return groundItems.Keys; } }
        //public virtual GroundItem GetGroundItem(int id)
        //{
        //    if (groundItems.ContainsKey(id))
        //    {
        //        return groundItems[id];
        //    }

        //    return null;
        //}


        //public virtual void AddGroundItem(GroundItem groundItem)
        //{
        //    if (groundItems.ContainsKey(groundItem.Id))
        //        return;

        //    groundItems.Add(groundItem.Id, groundItem);
        //}
        //public virtual void RemoveGroundItem(GroundItem groundItem)
        //{
        //    if (groundItems.ContainsKey(groundItem.Id))
        //        return;

        //    groundItems.Remove(groundItem.Id);
        //}

        protected Dictionary<int, Character> characters = new Dictionary<int, Character>();
        public Dictionary<int, Character>.KeyCollection Characters { get { return characters.Keys; } }

        public List<ArrowColltionArea> Projectiles { get; private set; }

        public virtual Character GetCharacter(int id)
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
            newCharacter.World = this;
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
            List<int> idList = new List<int>(characters.Keys.Count);
            idList.AddRange(characters.Keys);
            //var idList = characters.Keys;

            bool isRegenTick = gameTime.TotalGameTime.Ticks >= nextRegendTick;

            foreach (ushort characterId in idList)
            {
                if (!characters.ContainsKey(characterId))
                    continue;

                Character characterToUpdate = characters[characterId];
                characterToUpdate.Update(gameTime, this);

                if (isRegenTick)
                {
                    if (characterToUpdate.Stats.Health < Stats.Factor(characterToUpdate.Stats.MaxHealth, .9f) && characterToUpdate.Stats.Health >= Stats.Factor(characterToUpdate.Stats.MaxHealth, .1f))
                        characterToUpdate.Stats.Health += characterToUpdate.Stats.HealthRegen;

                    if (characterToUpdate.Stats.Energy < characterToUpdate.Stats.MaxEnergy)
                        characterToUpdate.Stats.Energy += characterToUpdate.Stats.EnergyRegen;

                    nextRegendTick = (gameTime.TotalGameTime + baseRegenTick).Ticks;
                }
            }

            List<ArrowColltionArea> toRemove = new List<ArrowColltionArea>();
            foreach (ArrowColltionArea projectile in this.Projectiles)
            {
                Vector2 start = projectile.Position.ToVector2();
                Vector2 newPosition = start;
                Vector2 end = projectile.Target.ToVector2();
                float distance = Vector2.Distance(start, end);
                Vector2 direction = Vector2.Normalize(end - start);

                newPosition += direction * projectile.Speed;
                if (Vector2.Distance(start, newPosition) >= distance)
                {
                    newPosition = end;
                    toRemove.Add(projectile);
                }

                projectile.Position = newPosition.ToPoint();
                if (projectile.GetProjectileAffected(this))
                    toRemove.Add(projectile);

            }
            foreach (var item in toRemove)
                this.Projectiles.Remove(item);
        }

        public virtual bool PerformAbility(CharacterPower ability, Character character)
        {
            if (!ability.CanBePerformedBy(character))
                return false;

            ability.PerformBy(this, character);
            return true;
        }
        public virtual bool PerformAbility(CharacterPowerIdentity abilityId, Character character)
        {
            if (character == null)
                return false;
            CharacterPower abilityToPerform = CharacterPower.Get(abilityId);
            if (abilityToPerform == null)
                return false;

            return this.PerformAbility(abilityToPerform, character);
        }

        protected abstract WorldMap GetCharactersMap(Character character);


        public double VectorToRadian(Vector2 direction)
        {
            if (direction != Vector2.Zero)
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