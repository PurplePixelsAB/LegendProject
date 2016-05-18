using LegendWorld.Data;
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
        private static Point defaultStartLocation = new Point(25, 25);

        public Character()
        {
            Stats = new Stats(this);
            Abilities = new List<CharacterPowerIdentity>();

            Position = Character.defaultStartLocation;
            MovingToPosition = Character.defaultStartLocation;
            AimToPosition = Character.defaultStartLocation;
            Health = 75; //ToDo: Remove/100
            Energy = 75; //ToDo: Remove/100

            CollitionArea = new CircleCollitionArea();
            CollitionArea.R = 20;
            CollitionArea.Position = this.Position;
            
            //Abilities.Add(AbilityIdentity.DefaultAttack);
            this.Learn(CharacterPowerIdentity.DefaultAttack);
        }
        
        public int Id { get; set; }
        
        public int CurrentMapId { get; set; }

        [DataMember]
        public Stats Stats { get; set; }

        //private byte GetWeaponPower()
        //{
        //    if (this.LeftHand == null)
        //        return 0;

        //    return this.LeftHand.Power;
        //}

        [DataMember]
        public virtual Point Position { get; protected set; }
        [DataMember]
        public Point MovingToPosition { get; protected set; }
        [DataMember]
        public Point AimToPosition { get; protected set; }

        [NotMapped]
        public bool IsMoving { get { return this.MovingToPosition != this.Position && !this.IsDead && this.MovingToPosition != null; } }

        public bool IsDead { get { return this.Health == 0; } }

        [DataMember]
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
        [DataMember]
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

        [DataMember]
        public byte MaxHealth { get { return this.Stats.GetStat(StatIdentifier.HealthMax); } }
        [DataMember]
        public byte MaxEnergy { get { return this.Stats.GetStat(StatIdentifier.EnergyMax); } }

        [NotMapped]
        public CircleCollitionArea CollitionArea { get; set; }

        [DataMember]
        public BagItem Inventory { get; set; }
        [DataMember]
        public List<CharacterPowerIdentity> Abilities { get; set; }
        [DataMember]
        public List<WeaponItem> Holster { get; set; }
        [DataMember]
        public ArmorItem Armor { get; set; }
        [DataMember]
        public WeaponItem LeftHand { get; set; }
        [DataMember]
        public WeaponItem RightHand { get; set; }

        [DataMember]
        public double BusyDuration { get; internal set; }
        [NotMapped]
        public bool IsBusy { get { return this.BusyDuration > 0D; } }

        [NotMapped]
        public PrepareAbility PrepareToPerform { get; set; }

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

        public event EventHandler AbilityLearning;
        public bool Learn(CharacterPowerIdentity ability)
        {
            if (this.Abilities.Count >= 6)
                return false;

            if (this.Abilities.Contains(ability))
                return false;

            this.Abilities.Add(ability);
            this.OnAbilityLearning(ability);
            return true;
        }
        protected virtual void OnAbilityLearning(CharacterPowerIdentity ability)
        {
            if (this.AbilityLearning != null)
                this.AbilityLearning(this, new EventArgs());
        }

        internal bool HasItemEquiped(ItemData.ItemIdentity requiredItem)
        {
            if (this.Armor.Data.Identity == requiredItem)
                return true;

            if (this.Holster.Any(weap => weap.Data.Identity == requiredItem))
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

        public event EventHandler<HealthChangedEventArgs> HealthChanged;
        private void OnHealthChange(byte oldHp)
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
