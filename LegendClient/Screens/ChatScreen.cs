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
    public class ChatScreen : Screen
    {
        //private readonly Keys[] ignoreKeys = new Keys[] { Keys.Enter };

        private SpriteBatch spriteBatch;
        //private Point centerScreen;
        private SpriteFont menuSpriteFont;
        private int selectedIndex = 0;
        private Vector2 fontSizeVector;
        private KeyboardState keyState;
        private KeyboardState oldKeyState;
        private string textMessage = string.Empty;
        private int playerId;
        private Vector2 textPosition;

        public ChatScreen(int characterID)
        {
            playerId = characterID;
        }
        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            //centerScreen = graphicsDevice.Viewport.Bounds.Center;
            textPosition = new Vector2(100, graphicsDevice.Viewport.Bounds.Height - 200);
            menuSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            fontSizeVector = new Vector2(0f, menuSpriteFont.LineSpacing);

            ActionKeyMapping actionKeyMappingSend = new ActionKeyMapping();
            actionKeyMappingSend.Id = 42;
            actionKeyMappingSend.Primary = Keys.Enter;
            actionKeyMappingSend.ActionTriggered += this.ActionKeyMappingSend;
            Input.Actions.Add(actionKeyMappingSend);

            ActionKeyMapping actionKeyMappingCancel = new ActionKeyMapping();
            actionKeyMappingCancel.Id = 42;
            actionKeyMappingCancel.Primary = Keys.Escape;
            actionKeyMappingCancel.ActionTriggered += this.ActionKeyMappingCancel;
            Input.Actions.Add(actionKeyMappingCancel);

            NetworkEngine.Instance.ToggleChat(playerId, true);
        }

        private void ActionKeyMappingCancel(object sender, ActionTriggeredEventArgs e)
        {
            NetworkEngine.Instance.ToggleChat(playerId, false);
            this.Close();
        }

        public override void UnloadContent()
        {
            menuSpriteFont = null;
            spriteBatch = null;
        }

        public override void Update(GameTime gameTime)
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();
            if (textMessage.Length < byte.MaxValue)
            {
                foreach (Keys key in keyState.GetPressedKeys())
                {
                    if (oldKeyState.IsKeyUp(key))
                    {
                        if (key == Keys.Back)
                        {
                            if (textMessage.Length >= 1)
                                textMessage = textMessage.Remove(textMessage.Length - 1, 1);
                        }
                        else if ((int)key >= 65 && (int)key <= 90)
                            textMessage += key.ToString().ToLower();
                        else if (key == Keys.Space)
                            textMessage += " ";
                        else if (key == Keys.OemComma)
                            textMessage += ",";
                        else if (key == Keys.OemPeriod)
                            textMessage += ".";
                        else if (key == Keys.OemQuestion)
                            textMessage += "?";
                        else if (key == Keys.D1 && keyState.IsKeyDown(Keys.LeftShift))
                            textMessage += "!";
                    }
                }
            }
        }
        private void ActionKeyMappingSend(object sender, ActionTriggeredEventArgs e)
        {
            if (textMessage.Length > 0)
                NetworkEngine.Instance.SendMessage(playerId, textMessage);

            this.Close();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(menuSpriteFont, "Say: " + textMessage, textPosition, Color.White);
            spriteBatch.End();
        }
    }
}
