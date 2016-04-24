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
        public override void Draw(GameTime gameTime)
        {
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            GameplayScreen screenToLoad = new GameplayScreen();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
