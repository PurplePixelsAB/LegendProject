using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
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
            Name = "Unknown";
            Position = new Point(1, 1);
            MovingToPosition = Position;
            AimToPosition = new Point(25, 25);
            Health = 75;
            MaxHealth = 100;
            Energy = 75;
            MaxEnergy = 100;
            CollitionArea = new CircleCollitionArea();
            CollitionArea.R = 20;
            CollitionArea.Position = this.Position;
            //Inventory = new BagItem();
            //Inventory.ItemsInBag.Add(new GoldItem() { StackCount = 1000 });
            Abilities = new List<AbilityIdentity>();
        }

        internal bool Teach(AbilityIdentity ability)
        {
            if (this.Abilities.Count >= 6)
                return false;

            this.Abilities.Add(ability);

            return true;
        }

        public ushort Id { get; set; }
        public ushort CurrentMapId { get; set; }

        public string Name { get; set; }

        public virtual Point Position { get; protected set; }
        public Point MovingToPosition { get; protected set; }
        public Point AimToPosition { get; protected set; }

        public CircleCollitionArea CollitionArea { get; set; }

        //public BagItem Inventory { get; set; }
        public ushort InventoryBagId { get; set; }

        public List<AbilityIdentity> Abilities { get; set; }

        public virtual void SetMoveToPosition(Point mapPoint)
        {
            if (MoveToIsValid(mapPoint))
            {
                this.MovingToPosition = mapPoint;
            }
        }

        public virtual void SetAimToPosition(Point mapPoint)
        {
            this.AimToPosition = mapPoint;
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

        public bool IsMoving { get { return this.MovingToPosition != this.Position; } }

        public bool IsDead {  get { return this.Health == 0; } }

        private byte health = 100;
        //public byte Health { get; set; }
        public byte MaxHealth { get; set; }
        public byte MaxEnergy { get; set; }
        public byte Energy { get; set; }
        
        public void UpdateMapPosition(GameTime gameTime)
        {
            if (!IsMoving)
                return;

            if (health == 0)
                return;

            Vector2 start = this.Position.ToVector2();
            Vector2 end = this.MovingToPosition.ToVector2();
            float distance = Vector2.Distance(start, end);
            Vector2 direction = Vector2.Normalize(end - start);
            Vector2 newPosition = this.Position.ToVector2();

            newPosition += direction * 20f; //this.MaxSpeed;//* ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 60000f);
            if (Vector2.Distance(start, newPosition) >= distance)
            {
                newPosition = end;
                this.MovingToPosition = newPosition.ToPoint();
            }

            this.Position = newPosition.ToPoint();
            this.CollitionArea.Position = this.Position;
        }

        public ushort Acceleration { get; set; } //PixelsPerSecound
        public ushort MaxSpeed { get; set; } //PixelsPerSecound 

        public byte Health
        {
            get
            {
                return health;
            }

            set
            {
                var oldHp = health;
                health = value;
                this.OnHealthChange(oldHp);
            }
        }

        public bool IsVisible { get; set; }

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
