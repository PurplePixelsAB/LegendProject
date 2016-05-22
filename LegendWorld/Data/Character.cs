﻿using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using LegendWorld.Data.Modifiers;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data.World
{
    public class Character : ICanMove, IDamagable
    {
        private const float lootDistance = 40f;
        private const int maxPowers = 5;
        //private static Point defaultStartLocation = new Point(25, 25);

        public Character(int id) : this(id, Point.Zero)
        {
        }
        public Character(int id, Point startPosition)
        {
            this.Id = id;
            Stats = new Stats(this);
            Powers = new List<CharacterPowerIdentity>();

            Position = startPosition; // Character.defaultStartLocation;
            MovingToPosition = startPosition; // Character.defaultStartLocation;
            AimToPosition = startPosition; // Character.defaultStartLocation;
            Health = 100; //ToDo: Remove/100
            Energy = 100; //ToDo: Remove/100

            CollitionArea = new CircleCollitionArea();
            CollitionArea.R = 20;
            CollitionArea.Position = this.Position;

            //Abilities.Add(AbilityIdentity.DefaultAttack);
            //this.Learn(CharacterPowerIdentity.DefaultAttack);
        }


        //public CharacterData Data { get; set; }

        public int Id { get; private set; }

        public int CurrentMapId { get; set; }

        public Stats Stats { get; set; }

        //private byte GetWeaponPower()
        //{
        //    if (this.LeftHand == null)
        //        return 0;

        //    return this.LeftHand.Power;
        //}

        public virtual Point Position { get; protected set; }

        public Point MovingToPosition { get; protected set; }

        public Point AimToPosition { get; protected set; }

        public bool IsMoving { get { return this.MovingToPosition != this.Position && this.MovingToPosition != null; } }

        public bool IsDead { get { return this.Health <= 0; } }

        public int Health
        {
            get
            {
                return this.Stats.GetStat(StatIdentifier.Health);
            }

            set
            {
                var oldHp = this.Stats.GetStat(StatIdentifier.Health); //ToDo: Move to Stats.Update() routine. Move Event to Stats.
                if (oldHp != value)
                {
                    this.Stats.Set(StatIdentifier.Health, value);
                    this.OnHealthChange(oldHp);
                }
            }
        }

        public int Energy
        {
            get
            {
                return this.Stats.GetStat(StatIdentifier.Energy);
            }

            set
            {
                //var oldEnergy = this.Stats.GetStat(StatIdentifier.Energy); //ToDo: Move to Stats.Update() routine. Move Event to Stats.
                this.Stats.Set(StatIdentifier.Energy, value);
                //this.OnEnergyChange(oldEnergy); //ToDo: Add to Stats as event.
            }
        }


        public int MaxHealth { get { return this.Stats.GetStat(StatIdentifier.HealthMax); } }

        public int MaxEnergy { get { return this.Stats.GetStat(StatIdentifier.EnergyMax); } }


        public CircleCollitionArea CollitionArea { get; set; }

        public event EventHandler<PerformsPowerEventArgs> PerformsPower;
        internal virtual void OnPerformsPower(CharacterPower characterPower)
        {
            if (this.PerformsPower != null)
            {
                PerformsPowerEventArgs eventArgs = new PerformsPowerEventArgs(characterPower);
                this.PerformsPower(this, eventArgs);
            }
        }

        public event EventHandler<AffectedByPowerEventArgs> AffectedByPower;
        internal void OnAffectedByPower(CharacterPower characterPower, Character abilityPerformedBy)
        {
            if (this.AffectedByPower != null)
            {
                AffectedByPowerEventArgs eventArgs = new AffectedByPowerEventArgs(characterPower, abilityPerformedBy);
                this.AffectedByPower(this, eventArgs);
            }
        }

        public List<CharacterPowerIdentity> Powers { get; set; }

        //public List<WeaponItem> Holster { get; set; }

        public ArmorItem Armor { get; set; }

        public WeaponItem LeftHand { get; set; }

        public WeaponItem RightHand { get; set; }


        public double BusyDuration { get; internal set; }


        public bool IsBusy { get { return this.BusyDuration > 0D; } }


        public PrepareAbility PrepareToPerform { get; set; }
        public ItemData InventoryData { get; set; }


        public event EventHandler MoveToChanged;
        public virtual void SetMoveToPosition(Point mapPoint)
        {
            if (MoveToIsValid(mapPoint))
            {
                this.MovingToPosition = mapPoint;
                this.OnMoveToChanged();
            }
        }

        protected virtual void OnMoveToChanged()
        {
            if (this.MoveToChanged != null)
                this.MoveToChanged(this, new EventArgs());
        }

        public event EventHandler AimToChanged;
        public virtual void SetAimToPosition(Point mapPoint)
        {
            this.AimToPosition = mapPoint;
            this.OnAimToChanged();
        }

        protected virtual void OnAimToChanged()
        {
            if (this.AimToChanged != null)
                this.AimToChanged(this, new EventArgs());
        }

        //public event EventHandler AbilityLearning;
        public bool Learn(CharacterPowerIdentity power)
        {
            if (power == CharacterPowerIdentity.DefaultAttack)
                return false;

            if (this.Powers.Count >= maxPowers)
                return false;

            if (this.Powers.Contains(power))
                return false;

            this.Powers.Add(power);
            this.OnPowerLearning(power);
            return true;
        }
        protected virtual void OnPowerLearning(CharacterPowerIdentity power)
        {
            //if (this.AbilityLearning != null)
            //    this.AbilityLearning(this, new EventArgs());
        }

        internal bool HasItemEquiped(ItemData.ItemIdentity requiredItem)
        {
            if (this.Armor != null)
            {
                if (this.Armor.Data.Identity == requiredItem)
                    return true;
            }

            //if (this.Holster.Any(weap => weap.Data.Identity == requiredItem))
            //    return true;

            if (this.RightHand != null)
            {
                if (this.RightHand.Data.Identity == requiredItem)
                    return true;
            }
            if (this.LeftHand != null)
            {
                if (this.LeftHand.Data.Identity == requiredItem)
                    return true;
            }

            return false;
        }

        public event EventHandler<MoveToMapPointValidatingEventArgs> MoveToMapPointValidating;
        protected virtual MoveToMapPointValidatingEventArgs OnMoveToMapPointValidating(Point mapPoint)
        {
            MoveToMapPointValidatingEventArgs eventArgs = new MoveToMapPointValidatingEventArgs();
            eventArgs.MoveToMapPoint = mapPoint;
            eventArgs.IsValid = true;
            if (this.MoveToMapPointValidating != null)
            {
                this.MoveToMapPointValidating(this, eventArgs);
            }

            return eventArgs;
        }
        private bool MoveToIsValid(Point mapPoint)
        {
            var e = this.OnMoveToMapPointValidating(mapPoint);
            return e.IsValid;
        }

        public void Update(GameTime gameTime)
        {
            this.Stats.Update(gameTime);
            this.Stats.Modifiers.Update(gameTime, this);
            if (this.BusyDuration > 0)
            {
                this.BusyDuration -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (this.IsMoving)
            {
                this.UpdateMapPosition(gameTime);
            }
        }
        private void UpdateMapPosition(GameTime gameTime)
        {
            if (!IsMoving)
                return;

            Vector2 start = this.Position.ToVector2();
            Vector2 newPosition = start;
            Vector2 end = this.MovingToPosition.ToVector2();
            float distance = Vector2.Distance(start, end);
            Vector2 direction = Vector2.Normalize(end - start);

            newPosition += this.Stats.CalculateMovement(direction);
            if (Vector2.Distance(start, newPosition) >= distance)
            {
                newPosition = end;
                this.MovingToPosition = newPosition.ToPoint();
            }

            this.Position = newPosition.ToPoint();
            this.CollitionArea.Position = this.Position;
        }

        public virtual bool Pickup(IItem itemUsed)
        {
            if (!itemUsed.Data.IsWorldItem)
                return false;
            if (!this.IsPositionInRange(itemUsed.Data.WorldLocation))
                return false;

            itemUsed.Data.MoveTo(this.InventoryData);
            return true;
        }
        public bool Equip(IItem itemToEquip)
        {
            if (itemToEquip.Category == ItemCategory.Weapon)
                return this.EquipWeapon((WeaponItem)itemToEquip);
            else if (itemToEquip.Category == ItemCategory.Armor)
                return this.EquipArmor((ArmorItem)itemToEquip);
            else
                return false;
        }

        private bool EquipArmor(ArmorItem itemToEquip)
        {
            if (this.Armor == itemToEquip)
                this.Armor = null;
            else
                this.Armor = itemToEquip;

            return true;
        }

        private bool EquipWeapon(WeaponItem itemToEquip)
        {
            if (this.RightHand == itemToEquip)
                this.RightHand = null;
            else
                this.RightHand = itemToEquip;

            return true;
        }
        public bool IsEquiped(IItem itemToCheck)
        {
            return this.Armor == itemToCheck || this.RightHand == itemToCheck || this.LeftHand == itemToCheck;
        }
        public bool IsEquiped(ItemData.ItemIdentity itemIdentity)
        {
            if (this.RightHand != null)
            {
                if (this.RightHand.Data.Identity == itemIdentity)
                    return true;
            }
            else if (this.LeftHand != null)
            {
                if (this.LeftHand.Data.Identity == itemIdentity)
                    return true;
            }
            else if (this.Armor != null)
            {
                if (this.Armor.Data.Identity == itemIdentity)
                    return true;
            }

            return false;
        }

        public bool IsPositionInRange(Point itemPosition)
        {
            Vector2 characterPosition = this.Position.ToVector2();
            float distance = Vector2.Distance(characterPosition, itemPosition.ToVector2());
            return distance <= lootDistance;
        }

        public event EventHandler<HealthChangedEventArgs> HealthChanged;
        private void OnHealthChange(int oldHp)
        {
            if (this.HealthChanged != null)
            {
                this.HealthChanged(this, new HealthChangedEventArgs() { PreviousHelth = oldHp });
            }
        }

        public class HealthChangedEventArgs : EventArgs
        {
            public HealthChangedEventArgs()
            {
            }

            public int PreviousHelth { get; set; }
        }

        public class MoveToMapPointValidatingEventArgs : EventArgs
        {
            public MoveToMapPointValidatingEventArgs()
            {
            }

            public bool IsValid { get; set; }
            public Point MoveToMapPoint { get; set; }
        }
    }
}
