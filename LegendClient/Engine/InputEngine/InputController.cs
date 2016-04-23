using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Engine.ScreenEngine;

namespace Engine.InputEngine
{
    public class InputController
    {
        internal InputController(Screen screen)
        {
            this.Actions = new List<IAction>(30);
            this.Screen = screen;
            //this.Manager = screen.Manager.Manager.InputManager; //legendaryManager.InputManager;           
        }

        private Screen Screen { get; set; }
        public List<IAction> Actions { get; private set; }
        //private InputManager Manager { get; set; }

        internal void Update(GameTime gameTime, InputManager updatingManager)
        {
            if (!Screen.Game.IsActive)
                return;

            foreach (IAction action in Actions)
            {
                if (action is ActionKeyMapping)
                {
                    if (updatingManager.IsActionTriggered((ActionKeyMapping)action))
                        this.OnActionTriggered(action, gameTime);
                    else if (updatingManager.IsActionPressed((ActionKeyMapping)action))
                        this.OnActionPressed(action, gameTime);
                    else if (updatingManager.IsActionReleased((ActionKeyMapping)action))
                        this.OnActionReleased(action, gameTime);
                }
                else if (action is ActionButtonAxisMapping)
                {
                    if (updatingManager.IsAxisEntering((ActionButtonAxisMapping)action))
                        this.OnAxisEnter(action, gameTime);
                    else if (updatingManager.IsAxisLeaving((ActionButtonAxisMapping)action))
                        this.OnAxisLeave(action, gameTime);
                    else if (updatingManager.IsActionTriggered((ActionButtonAxisMapping)action))
                        this.OnActionTriggered(action, gameTime);
                    else if (updatingManager.IsActionPressed((ActionButtonAxisMapping)action))
                        this.OnActionPressed(action, gameTime);
                }
                else if (action is ActionButtonMapping)
                {
                    if (updatingManager.IsActionTriggered((ActionButtonMapping)action))
                        this.OnActionTriggered(action, gameTime);
                    else if (updatingManager.IsActionPressed((ActionButtonMapping)action))
                        this.OnActionPressed(action, gameTime);                    
                }
            }
        }

        public event ActionTriggeredEventHandler ActionTriggered;
        private void OnActionTriggered(IAction action, GameTime gameTime)
        {
            if (ActionTriggered != null)
                ActionTriggered(this, new ActionTriggeredEventArgs(action, gameTime));

            if (action is ActionKeyMapping)
                ((ActionKeyMapping)action).OnActionTrigger(gameTime);
            else if (action is ActionButtonMapping)
                ((ActionButtonMapping)action).OnActionTrigger(gameTime);

        }
        public event ActionTriggeredEventHandler ActionReleased;
        private void OnActionReleased(IAction action, GameTime gameTime)
        {
            if (ActionReleased != null)
                ActionReleased(this, new ActionTriggeredEventArgs(action, gameTime));

            if (action is ActionKeyMapping)
                ((ActionKeyMapping)action).OnActionReleased(gameTime);
            else if (action is ActionButtonMapping)
                ((ActionButtonMapping)action).OnActionReleased(gameTime);

        }

        public event ActionPressedEventHandler ActionPressed;
        private void OnActionPressed(IAction action, GameTime gameTime)
        {
            if (ActionPressed != null)
                ActionPressed(this, new ActionPressedEventArgs(action, gameTime));

            if (action is ActionKeyMapping)
                ((ActionKeyMapping)action).OnActionPressed(gameTime);
            else if (action is ActionButtonMapping)
                ((ActionButtonMapping)action).OnActionPressed(gameTime);
        }

        public event AxisEnterEventHandler AxisEnter;
        private void OnAxisEnter(IAction action, GameTime gameTime)
        {
            if (AxisEnter != null)
                AxisEnter(this, new AxisEnterEventArgs(action, gameTime));

            if (action is ActionButtonAxisMapping)
                ((ActionButtonAxisMapping)action).OnAxisEnter(gameTime);

        }

        public event AxisLeaveEventHandler AxisLeave;
        private void OnAxisLeave(IAction action, GameTime gameTime)
        {
            if (AxisLeave != null)
                AxisLeave(this, new AxisLeaveEventArgs(action, gameTime));

            if (action is ActionButtonAxisMapping)
                ((ActionButtonAxisMapping)action).OnAxisLeave(gameTime);
        }
    }
}
