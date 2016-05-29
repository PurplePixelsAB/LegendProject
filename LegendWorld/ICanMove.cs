using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface ICanMove : IHasPosition
    {
        Point MovingToPosition { get; }
        void SetMoveToPosition(Point mapPoint);
        event EventHandler<World.Character.MoveToMapPointValidatingEventArgs> MoveToMapPointValidating;
        void Update(GameTime gameTime, WorldState world);
    }
}
