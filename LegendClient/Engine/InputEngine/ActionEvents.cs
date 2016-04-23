using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.InputEngine
{
    public class BaseActionEvent : EventArgs
    {
        public BaseActionEvent(IAction action, GameTime gameTime)
        { 
            this.Action = action;
            this.GameTime = gameTime;
            //ToDo: Improvments, get States from InputManager.
            this.MouseState = Mouse.GetState();
            this.KeyboardState = Keyboard.GetState();
        }
        public IAction Action { get; private set; }
        public GameTime GameTime { get; private set; }
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }
    }

    public class ActionTriggeredEventArgs : BaseActionEvent
    {
        public ActionTriggeredEventArgs(IAction action, GameTime gameTime) : base(action, gameTime) { }
    }
    public class ActionPressedEventArgs : BaseActionEvent
    {
        public ActionPressedEventArgs(IAction action, GameTime gameTime) : base(action, gameTime) { }
    }

    public delegate void ActionTriggeredEventHandler(object sender, ActionTriggeredEventArgs e);
    public delegate void ActionPressedEventHandler(object sender, ActionPressedEventArgs e);

    public class AxisEnterEventArgs : BaseActionEvent
    {
        public AxisEnterEventArgs(IAction action, GameTime gameTime) : base(action, gameTime) { }
    }
    public class AxisLeaveEventArgs : BaseActionEvent
    {
        public AxisLeaveEventArgs(IAction action, GameTime gameTime) : base(action, gameTime) { }
    }

    public delegate void AxisEnterEventHandler(object sender, AxisEnterEventArgs e);
    public delegate void AxisLeaveEventHandler(object sender, AxisLeaveEventArgs e);
}
