using Engine.ScreenEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsClient;

namespace LegendClient.Screens
{
    internal class LoadingScreen : Screen
    {
        private Point centerScreen;
        private SpriteFont loadingSpriteFont;
        private Screen screenToLoad;
        private SpriteBatch spriteBatch;
        private List<char> dots = new List<char>(20);
        private TimeSpan dotInterval = new TimeSpan(0, 0, 0, 0, 1500);
        private TimeSpan nextDot;
        //private Task loadingTask;
        //private bool isLoadCompleated;
        private bool isOnlyScreen = false;

        public override void Draw(GameTime gameTime)
        {
            if (this.Manager.ScreenCount == 1 && this.Visible)
            {
                isOnlyScreen = true;
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(loadingSpriteFont, "Loading." + new String(dots.ToArray()), centerScreen.ToVector2(), Color.White);
            spriteBatch.End();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            centerScreen = graphicsDevice.Viewport.Bounds.Center;
            loadingSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            //screenToLoad = new GameplayScreen();
            //loadingTask = Task.Factory.StartNew(() => screenToLoad.LoadContent(graphicsDevice)); //Causes double LoadContent event.
        }

        public override void UnloadContent()
        {

        }

        public LoadingScreen(Screen screenToLoad)
        {
            this.screenToLoad = screenToLoad;
        }

        //private bool screenIsLoaded = false;
        public override void Update(GameTime gameTime)
        {

            if (isOnlyScreen)
            {
                this.Close();

                screenToLoad.Initialize(this.Manager);
                screenToLoad.Activate();

                this.Game.ResetElapsedTime();
            }
            

            //if (nextDot <= gameTime.TotalGameTime)
            //{
            //    dots.Add('.');
            //    nextDot = gameTime.TotalGameTime.Add(dotInterval);
            //    if (dots.Count >= 10)
            //        dots.Clear();
            //}

            //if (screenToLoad.IsConnected) // && loadingTask.IsCompleted)
            //{
            //    //if (!charIsSelected)
            //    //{
            //    //    //Random rnd = new Random();
            //    //    screenToLoad.SelectCharacter((ushort)rnd.Next(1, UInt16.MaxValue));
            //    //}
            //    //else if (screenToLoad.IsLoaded)
            //    //{
            //    screenToLoad.Activate();
            //    isLoadCompleated = true;
            //    this.Close();
            //    //}
            //}
            
        }
    }
}
