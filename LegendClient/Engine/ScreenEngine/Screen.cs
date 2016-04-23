using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//using Engine.MenuEngine;
using Engine.InputEngine;

namespace Engine.ScreenEngine
{
    public abstract class Screen
    {
        public Screen()
        {
        }
        
        public void Activate()
        {
            if (Manager != null)
            {
                if (!Manager.Contains(this))
                    Manager.Add(this);

                this.Visible = true;
            }
            else
                throw new NullReferenceException("ScreenManager is not Initilized!");
        }
        public void Kill()
        {
            if (Manager == null)
                throw new NullReferenceException("ScreenManager is not Initilized!");

            //if (MenuManager == null)
            //    throw new NullReferenceException("MenuManager is not Initilized!");

            //IScreenControl[] menuControls = Controls.ToArray();
            //Controls.Clear();
            //for (int i = 0; i < menuControls.Length; i++)
            //{
            //    MenuManager.Remove(menuControls[i]);
            //    menuControls[i].Dispose();
            //}

            Manager.Remove(this);
        }

        public void Show()
        {
            //Todo: Add Transition Code
            this.Activate();
        }
        public void Close()
        {
            //Todo: Add Transition Code
            this.Kill();
        }
        public void Hide()
        {
            this.Visible = false;
        }

        //internal List<IScreenControl> Controls = new List<IScreenControl>();

        private bool isVisible = false;
        public bool Visible 
        { 
            get { return isVisible; }
            set
            {
                isVisible = value;

                //foreach (IScreenControl control in Controls)
                //{
                //    control.Visible = isVisible;
                //}
            }
        }


        public bool HasFocus { get; internal set; }

        protected internal ScreenManager Manager { get; private set; }
        protected internal Game Game { get { return Manager.Game; } }
        //protected internal SpriteBatch SpriteBatch { get { return Manager.SpriteBatch; } }
        //protected internal GraphicsDevice GraphicsDevice { get { return Manager.GraphicsDevice; } }

        public virtual void Initialize(ScreenManager screenManager)
        {
            Manager = screenManager;
            Input = new InputController(this);
        }
        public abstract void LoadContent(GraphicsDevice graphicsDevice);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        //public abstract void UpdateInput(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public InputController Input;
        internal void UpdateInput(GameTime gameTime, InputManager inputManager)
        {
            if (Input == null)
                throw new NullReferenceException("Input is Null, Initialize must be called before UpdateInput.");

            if (Input.Actions.Count == 0)
                return;

            Input.Update(gameTime, inputManager);
        }
    } 
}
