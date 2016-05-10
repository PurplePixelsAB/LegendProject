using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using LegendWorld.Data.Modifiers;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Data.World
{
    public class Character : ICanMove, INamable, IDamagable
    {
        public Character()
        {
            Stats = new Stats(this);
            Abilities = new List<AbilityIdentity>();

            Position = new Point(1, 1);
            MovingToPosition = Position;
            AimToPosition = new Point(25, 25);
            Health = 75;
            Energy = 75;

            CollitionArea = new CircleCollitionArea();
            CollitionArea.R = 20;
            CollitionArea.Position = this.Position;
            
            //Stats.StatModify += Stats_StatModify;
            Abilities.Add(AbilityIdentity.DefaultAttack);
        }

        //private void Stats_StatModify(object sender, Stats.StatModifyEventArgs e)
        //{
        //    foreach (CharacterModifier modifier in this.Modifiers)
        //    {
        //        modifier.OnStatModify(e.Stat, e.NewValue, e.OldValue);
        //    }
        //}

        public int Id { get; set; }


        public int CurrentMapId { get; set; }

        public string Name { get; set; }

        public Stats Stats { get; set; }

        //internal byte GetDamageFromPower(int abilityBasePower)
        //{
        //    byte returnDamage = 0;
        //    int weaponPower = this.GetWeaponPower();

        //    int totalBasePower = weaponPower + abilityBasePower;
        //    //int modifiedPower = 0;

        //    //foreach (var modifier in this.Modifiers)
        //    //{
        //    //    modifiedPower += modifier.ModifyPower(totalBasePower);
        //    //}

        //    this.Modifiers.

        //    returnDamage = (byte)MathHelper.Clamp(totalBasePower + modifiedPower, byte.MinValue, byte.MaxValue);
        //    return returnDamage;
        //}


        private byte GetWeaponPower()
        {
            if (this.WeaponInHand == null)
                return 0;

            return this.WeaponInHand.Power;
        }

        public virtual Point Position { get; protected set; }
        public Point MovingToPosition { get; protected set; }
        public Point AimToPosition { get; protected set; }

        public bool IsMoving { get { return this.MovingToPosition != this.Position && !this.IsDead && this.MovingToPosition != null; } }

        public bool IsDead { get { return this.Health == 0; } }

        public byte Health
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
                    this.Stats.Modify(StatIdentifier.Health, value);
                    this.OnHealthChange(oldHp);
                }
            }
        }
        public byte Energy
        {
            get
            {
                return this.Stats.GetStat(StatIdentifier.Energy);
            }

            set
            {
                //var oldEnergy = this.Stats.GetStat(StatIdentifier.Energy); //ToDo: Move to Stats.Update() routine. Move Event to Stats.
                this.Stats.Modify(StatIdentifier.Energy, value);
                //this.OnEnergyChange(oldEnergy); //ToDo: Add to Stats as event.
            }
        }
        public byte MaxHealth { get { return this.Stats.GetStat(StatIdentifier.HealthMax); } }
        public byte MaxEnergy { get { return this.Stats.GetStat(StatIdentifier.EnergyMax); } }

        public CircleCollitionArea CollitionArea { get; set; }

        //public BagItem Inventory { get; set; }
        public int InventoryBagId { get; set; }

        public List<AbilityIdentity> Abilities { get; set; }
        public List<WeaponItem> Holster { get; set; }
        public ArmorItem Armor { get; set; }
        public WeaponItem WeaponInHand { get; set; }

        //public Ability Performing { get; set; }

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

        internal bool Teach(AbilityIdentity ability)
        {
            if (this.Abilities.Count >= 6)
                return false;

            if (this.Abilities.Contains(ability))
                return false;

            this.Abilities.Add(ability);

            return true;
        }

        internal bool HasItemEquiped(ItemIdentity requiredItem)
        {
            if (this.Armor.Identity == requiredItem)
                return true;

            if (this.Holster.Any(weap => weap.Identity == requiredItem))
                return true;

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


        //public bool IsVisible { get; set; }
        public double BusyDuration { get; internal set; }
        public bool IsBusy { get { return this.BusyDuration > 0D; } }

        //public float MovementSpeed { get; internal set; }

        public event EventHandler<HealthChangedEventArgs> HealthChanged;
        private void OnHealthChange(byte oldHp)
        {
            if (this.HealthChanged != null)
            {
                this.HealthChanged(this, new HealthChangedEventArgs() { PreviousHelth = oldHp });
            }
        }

        //public void ApplyDamage(byte damageAmount)
        //{
        //    if (this.HasModifier(typeof(AbsorbDamageModifier)))
        //    {
        //        AbsorbDamageModifier absorbDamageModifier = (AbsorbDamageModifier)this.Modifiers.First(m => m is AbsorbDamageModifier);
        //    }
        //}

        public class HealthChangedEventArgs : EventArgs
        {
            public HealthChangedEventArgs()
            {
            }

            public byte PreviousHelth { get; set; }
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
