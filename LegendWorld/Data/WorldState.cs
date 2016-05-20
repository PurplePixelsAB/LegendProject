using Data;
using Data.World;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static Data.ItemData;

namespace Network
{
    public abstract class WorldState
    {
        //protected byte swingDmg = 24;
        //protected byte swingEnergy = 34;
        //protected byte healAmount = 11;

        protected TimeSpan baseRegenTick = new TimeSpan(0, 0, 1);
        //protected TimeSpan baseEnergyTick = new TimeSpan(0, 0, 1);
        private long nextRegendTick = 0;

        protected Dictionary<int, IItem> items = new Dictionary<int, IItem>();
        public Dictionary<int, IItem>.KeyCollection Items { get { return items.Keys; } }

        public WorldState()
        {
            CharacterPower.Load();

            this.Projectiles = new List<ArrowColltionArea>(30);
            //ushort itemId = 1;
            //foreach (AbilityIdentity abilityId in Enum.GetValues(typeof(AbilityIdentity)))
            //{
            //    Random rnd = new Random();
            //    for (ushort i = 0; i < 10; i++)
            //    {
            //        AbilityScrollItem item = new AbilityScrollItem();
            //        item.Id = itemId;
            //        item.Ability = abilityId;
            //        itemId++;
            //        this.AddItem(item);

            //        GroundItem groundItem = new GroundItem();
            //        groundItem.ItemId = item.Id;
            //        groundItem.Id = itemId;
            //        groundItem.Position = new Point(rnd.Next(1, 1000), rnd.Next(1, 1000));
            //        this.AddGroundItem(groundItem);

            //        itemId++;
            //    }
            //}
        }

        public virtual IItem GetItem(int id)
        {
            if (items.ContainsKey(id))
            {
                return items[id];
            }

            return null;
        }
        public virtual void AddItem(IItem item)
        {
            if (items.ContainsKey((ushort)item.Data.ItemDataID))
                return;

            items.Add((ushort)item.Data.ItemDataID, item);
        }
        public virtual void RemoveItem(IItem item)
        {
            if (!items.ContainsKey((ushort)item.Data.ItemDataID))
                return;

            items.Remove(item.Data.ItemDataID);
            if (item.Data.ContainerID.HasValue)
            {
                ContainerItem container = (ContainerItem)this.GetItem(item.Data.ContainerID.Value);
                if (container != null)
                {
                    if (container.Items.Contains(item))
                        container.Items.Remove(item);
                }
            }
            item.Data.ContainerID = null;
            item.Data.WorldMapID = null;
            item.Data.WorldX = null;
            item.Data.WorldY = null;
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

        public IItem CreateItem(ItemData itemData)
        {
            IItemFactory factory = this.GetItemFactory(itemData.Identity);
            IItem newReturnItem = factory.CreateNew(itemData);

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
                characterToUpdate.Update(gameTime);

                if (isRegenTick)
                {
                    if (characterToUpdate.Health < characterToUpdate.MaxHealth - 10 && characterToUpdate.Health >= 10)
                        characterToUpdate.Health += 1;

                    if (characterToUpdate.Energy < characterToUpdate.MaxEnergy)
                        characterToUpdate.Energy += 1;

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
                projectile.GetProjectileAffected(this);
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