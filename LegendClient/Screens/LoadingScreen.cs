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
        GameplayScreen screenToLoad;
        private SpriteBatch spriteBatch;
        private List<char> dots = new List<char>(20);
        private TimeSpan dotInterval = new TimeSpan(0, 0, 0, 0, 1500);
        private TimeSpan nextDot;
        private Task loadingTask;
        private bool isLoadCompleated;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(loadingSpriteFont, "Loading." + new String(dots.ToArray()), centerScreen.ToVector2(), Color.White);
            spriteBatch.End();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            centerScreen = graphicsDevice.Viewport.Bounds.Center;
            loadingSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            screenToLoad = new GameplayScreen();
            screenToLoad.Initialize(this.Manager);
            loadingTask = Task.Factory.StartNew(() => screenToLoad.LoadContent(graphicsDevice));
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (isLoadCompleated)
                return;

            if (nextDot <= gameTime.TotalGameTime)
            {
                dots.Add('.');
                nextDot = gameTime.TotalGameTime.Add(dotInterval);
                if (dots.Count >= 10)
                    dots.Clear();
            }

            if (screenToLoad.IsConnected && loadingTask.IsCompleted)
            {
                Random rnd = new Random();
                screenToLoad.SelectCharacter(rnd.Next(1, Int16.MaxValue));
                screenToLoad.Activate();
                isLoadCompleated = true;
                this.Close();
            }
        }
    }
}
