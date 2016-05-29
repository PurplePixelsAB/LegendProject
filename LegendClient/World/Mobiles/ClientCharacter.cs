using Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Network;
using LegendWorld.Data;
using LegendClient.Screens;
using Data;

namespace WindowsClient.World.Mobiles
{
    public class ClientCharacter : Character
    {
        public Point DrawPosition { get; set; }
        //public BagClientItem Inventory { get; set; }

        public ClientCharacter(int id, Point startPosition) : base(id, startPosition)
        {
            this.DrawPosition = startPosition;
        }

        public override void SetMoveToPosition(Point mapPoint)
        {
            base.SetMoveToPosition(mapPoint);
        }

        public Vector2 OldDrawPosition { get; set; }
        internal void ServerMoveToRecived(Point point)
        {
            this.SetMoveToPosition(point);
        }

        internal void ServerStatsRecived(byte? health, byte? energy)
        {
            if (health.HasValue)
            {
                if (this.Stats.Health != health.Value)
                {
                    this.Stats.Health = health.Value; //this.Stats.Set(StatIdentifier.Health, health.Value);
                }
            }
            if (energy.HasValue)
            {
                if (this.Stats.Energy != energy.Value)
                {
                    this.Stats.Energy = energy.Value; //this.Stats.Set(StatIdentifier.Energy, energy.Value);
                }
            }
        }

        internal void ServerAimToRecived(Point point)
        {
            this.AimToPosition = point;
        }

        public override bool Pickup(IItem itemUsed)
        {
            if (base.Pickup(itemUsed))
            {
                this.Inventory.Items.Add(itemUsed);
                return true;
            }
            return false;
        }
    }
}