using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.InputEngine
{
    public interface IAction
    {
        int Action { get; set; }
        //void OnActionTrigger();
        //void OnActionPressed();
    }

    public class ActionKeyMapping : IAction
    {
        public int Action { get; set; }
        public Keys Primary { get; set; }
        public Keys PrimaryMod { get; set; }
        public Keys Secondary { get; set; }
        public Keys SecondaryMod { get; set; }

        public event ActionTriggeredEventHandler ActionTriggered;
        public event ActionTriggeredEventHandler ActionReleased;
        public event ActionPressedEventHandler ActionPressed;

        internal void OnActionTrigger(GameTime gameTime)
        {
            if (ActionTriggered != null)
                ActionTriggered(this, new ActionTriggeredEventArgs(this, gameTime));
        }
        internal void OnActionReleased(GameTime gameTime)
        {
            if (ActionReleased != null)
                ActionReleased(this, new ActionTriggeredEventArgs(this, gameTime));
        }
        internal void OnActionPressed(GameTime gameTime)
        {
            if (ActionPressed != null)
                ActionPressed(this, new ActionPressedEventArgs(this, gameTime));
        }
    }

    public class ActionButtonMapping : IAction
    {
        public int Action { get; set; }
        public MouseButtons Primary { get; set; }
        public Keys PrimaryMod { get; set; }
        public MouseButtons Secondary { get; set; }
        public Keys SecondaryMod { get; set; }

        public event ActionTriggeredEventHandler ActionTriggered;
        public event ActionTriggeredEventHandler ActionReleased;
        public event ActionPressedEventHandler ActionPressed;

        internal void OnActionTrigger(GameTime gameTime)
        {
            if (ActionTriggered != null)
                ActionTriggered(this, new ActionTriggeredEventArgs(this, gameTime));
        }
        internal void OnActionReleased(GameTime gameTime)
        {
            if (ActionReleased != null)
                ActionReleased(this, new ActionTriggeredEventArgs(this, gameTime));
        }
        internal void OnActionPressed(GameTime gameTime)
        {
            if (ActionPressed != null)
                ActionPressed(this, new ActionPressedEventArgs(this, gameTime));
        }
    }

    public class ActionButtonAxisMapping : ActionButtonMapping
    {
        public Rectangle ScreenPosition { get; set; }

        public event AxisEnterEventHandler AxisEnter;
        public event AxisLeaveEventHandler AxisLeave;

        internal void OnAxisEnter(GameTime gameTime)
        {
            if (AxisEnter != null)
                AxisEnter(this, new AxisEnterEventArgs(this, gameTime));
        }
        internal void OnAxisLeave(GameTime gameTime)
        {
            if (AxisLeave != null)
                AxisLeave(this, new AxisLeaveEventArgs(this, gameTime));
        }
    }
}
