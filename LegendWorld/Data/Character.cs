using Microsoft.Xna.Framework;
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
        }
        public int Id { get; set; }
        public int CurrentMapId { get; set; }

        public string Name { get; set; }

        public virtual Point Position { get; protected set; }
        public Point MovingToPosition { get; protected set; }
        public Point AimToPosition { get; protected set; }

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

        private byte health = 100;
        //public byte Health { get; set; }
        public byte MaxHealth { get; set; }
        public byte MaxEnergy { get; set; }
        public byte Energy { get; set; }

        //protected Vector2 moveForce = new Vector2();
        //protected Vector2 mapPosition = new Vector2();
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

            //float accelPixelsPerDelta = (this.Acceleration / 60000f) * gameTime.ElapsedGameTime.Milliseconds;
            ////Vector2 moveForce = this.MoveForce.ToVector2(); // new Vector2();
            //if (this.MapPoint.X != this.MovingToPoint.X)
            //{
            //    if (this.MapPoint.X < this.MovingToPoint.X && MoveForce.X != this.MaxSpeed)
            //    {                    
            //        moveForce.X += accelPixelsPerDelta;
            //    }
            //    else if (MoveForce.X != 0 - this.MaxSpeed)
            //    {
            //        moveForce.X -= accelPixelsPerDelta;
            //    }
            //}
            //else
            //{
            //    moveForce.X = 0f;
            //}
            //if (this.MapPoint.Y != this.MovingToPoint.Y)
            //{
            //    if (this.MapPoint.Y < this.MovingToPoint.Y && MoveForce.Y != this.MaxSpeed)
            //    {
            //        moveForce.Y += accelPixelsPerDelta;
            //    }
            //    else if (MoveForce.Y != 0 - this.MaxSpeed)
            //    {
            //        moveForce.Y -= accelPixelsPerDelta;
            //    }
            //}
            //else
            //{
            //    moveForce.Y = 0f;
            //}


            //mapPosition += moveForce;

            //this.MoveForce = moveForce.ToPoint();

            ////int x = this.MapPoint.X, y = this.MapPoint.Y;

            //this.MapPoint = mapPosition.ToPoint();
        }

        public int Acceleration { get; set; } //PixelsPerSecound
        public int MaxSpeed { get; set; } //PixelsPerSecound 

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

        //public Point MoveForce { get; set; }

        //public void UpdatePosition(int x, int y)
        //{
        //    this.X = x;
        //    this.Y = y;
        //}

        //public void UpdateVelocity(int velocityX, int velocityY)
        //{
        //    this.VelocityY = velocityY;
        //    this.VelocityX = velocityX;
        //}

        //public void UpdateInput(bool up, bool down, bool left, bool right)
        //{
        //    this.Up = up;
        //    this.Down = down;
        //    this.Left = left;
        //    this.Right = right;
        //    this.OnInputUpdated();
        //}

        //public event EventHandler InputUpdated;
        //private void OnInputUpdated()
        //{
        //    if (this.InputUpdated != null)
        //    {
        //        this.InputUpdated(this, new EventArgs());
        //    }
        //}

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
