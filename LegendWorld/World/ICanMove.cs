﻿using Microsoft.Xna.Framework;
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
        void UpdateMapPosition(GameTime gameTime);
    }
}
