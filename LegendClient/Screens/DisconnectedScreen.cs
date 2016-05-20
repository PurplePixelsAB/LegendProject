using Engine.ScreenEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsClient.Net;
using LegendWorld.Data;
using Engine.InputEngine;
using Microsoft.Xna.Framework.Input;
using WindowsClient;
using Data;

namespace LegendClient.Screens
{
    public class DisconnectedScreen : Screen
    {
        private SpriteBatch spriteBatch;
        private Point centerScreen;
        private SpriteFont spriteFont;
        private Vector2 fontSizeVector;

        public string Message { get; private set; }

        public DisconnectedScreen(string disconnectMessage)
        {
            this.Message = disconnectMessage;
        }
        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            centerScreen = graphicsDevice.Viewport.Bounds.Center;
            spriteFont = Game.Content.Load<SpriteFont>("Damage");
            fontSizeVector = new Vector2(0f, spriteFont.LineSpacing);
        }

        public override void UnloadContent()
        {
            spriteFont = null;
            spriteBatch = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Disconnected, press ESC to exit Game.", centerScreen.ToVector2(), Color.White);
            spriteBatch.DrawString(spriteFont, this.Message, centerScreen.ToVector2() + fontSizeVector, Color.White);
            spriteBatch.End();
        }
    }
}
