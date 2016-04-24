using Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Network;

namespace WindowsClient.World.Mobiles
{
    public class ClientCharacter : Character
    {
        public override void SetMoveToPosition(Point mapPoint)
        {
            base.SetMoveToPosition(mapPoint);
        }

        public Vector2 DrawPosition { get; set; }

        public Point lastKnownServerPosition;
        internal void CheckServerPosition(long tick, Point point)
        {
            lastKnownServerPosition = point;
        }
    }
}