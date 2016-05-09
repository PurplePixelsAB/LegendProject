using Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Network;
using LegendWorld.Data;

namespace WindowsClient.World.Mobiles
{
    public class ClientCharacter : Character
    {
        public override void SetMoveToPosition(Point mapPoint)
        {
            base.SetMoveToPosition(mapPoint);
        }

        public Vector2 DrawPosition { get; set; }

        private Point lastKnownServerPosition;
        internal void ServerMoveToRecived(Point point)
        {

            lastKnownServerPosition = point;
        }

        internal void ServerStatsRecived(byte? health, byte? energy)
        {
            if (health.HasValue)
            {
                if (this.Health != health.Value)
                {
                    this.Stats.Modify(StatIdentifier.Health, health.Value);
                }
            }
            if (energy.HasValue)
            {
                if (this.Energy != energy.Value)
                {
                    this.Stats.Modify(StatIdentifier.Energy, energy.Value);
                }
            }
        }

        internal void ServerAimToRecived(Point point)
        {
            this.AimToPosition = point;
        }
    }
}