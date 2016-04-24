using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Engine.InputEngine;

namespace Engine.ScreenEngine
{
    public class ScreenManager
    {
        internal ScreenManager(Game game, InputManager inputManager)
        {
            this.Game = game;
            this.InputManager = inputManager;
        }

        private List<Screen> screenList = new List<Screen>(10);
        private List<Screen> screensToUpdate = new List<Screen>(5);
        internal void Add(Screen screen)
        {
            screenList.Add(screen);

            if (isInitialized && screen.Manager == null)
                screen.Initialize(this);

            if (isLoaded)
                screen.LoadContent(Game.GraphicsDevice);
        }
        internal void Remove(Screen screen)
        {
            screenList.Remove(screen);

            if (isLoaded)
                screen.UnloadContent();

            screen = null;
        }
        internal bool Contains(Screen screen)
        {
            return screenList.Contains(screen);
        }

        private bool isInitialized = false;
        private bool isLoaded = false;

        public Game Game { get; internal set; }
        public InputManager InputManager { get; private set; }

        protected internal void Initialize()
        {
            //base.Initialize();
            foreach (Screen screen in this.screenList)
            {
                screen.Initialize(this);
            }
            isInitialized = true;
        }

        protected internal void LoadContent()
        {
            foreach (Screen screen in this.screenList)
            {
                screen.LoadContent(Game.GraphicsDevice);
            }
            isLoaded = true;
        }
        protected internal void UnloadContent()
        {
            foreach (Screen screen in this.screenList)
            {
                screen.UnloadContent();
            }
        }

        protected internal void Update(GameTime gameTime)
        {
            screensToUpdate.Clear();

            foreach (Screen screen in this.screenList)
                screensToUpdate.Add(screen);

            bool HasFocus = Game.IsActive;
            //bool Covered = false; No need until Transitions

            for (int i = screensToUpdate.Count - 1; i >= 0; i--)
            {
                if (screensToUpdate[i].Visible)
                {
                    screensToUpdate[i].HasFocus = HasFocus;
                    if (HasFocus)
                    {
                        screensToUpdate[i].UpdateInput(gameTime, this.InputManager);
                        HasFocus = false;
                    }
                }
                else
                    screensToUpdate[i].HasFocus = false;

                screensToUpdate[i].Update(gameTime);

                screensToUpdate.RemoveAt(i);
            }
        }

        protected internal void Draw(GameTime gameTime)
        {
            foreach (Screen screen in this.screenList)
            {
                if (screen.Visible)
                    screen.Draw(gameTime);
            }
        }
    }
}
