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

namespace LegendClient.Screens
{
    public class SelectCharacterScreen : Screen
    {
        private SpriteBatch spriteBatch;
        private Point centerScreen;
        private SpriteFont characterSpriteFont;
        private NetworkEngine network;
        private bool charactersLoaded = false;
        private List<SelectableCharacter> characters;
        private int selectedIndex = 0;
        private Vector2 fontSizeVector;

        public SelectCharacterScreen(NetworkEngine networkEngine)
        {
            network = networkEngine;
        }
        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            centerScreen = graphicsDevice.Viewport.Bounds.Center;
            characterSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            fontSizeVector = new Vector2(0f, characterSpriteFont.LineSpacing);
            //network = new NetworkEngine();
            characters = network.GetCharacters();

            ActionKeyMapping actionKeyMappingUp = new ActionKeyMapping();
            actionKeyMappingUp.Id = 40;
            actionKeyMappingUp.Primary = Keys.Up;
            actionKeyMappingUp.ActionTriggered += ActionKeyMappingUp;
            Input.Actions.Add(actionKeyMappingUp);
            ActionKeyMapping actionKeyMappingDown = new ActionKeyMapping();
            actionKeyMappingDown.Id = 41;
            actionKeyMappingDown.Primary = Keys.Down;
            actionKeyMappingDown.ActionTriggered += this.ActionKeyMappingDown;
            Input.Actions.Add(actionKeyMappingDown);
            ActionKeyMapping actionKeyMappingSelect = new ActionKeyMapping();
            actionKeyMappingSelect.Id = 42;
            actionKeyMappingSelect.Primary = Keys.Enter;
            actionKeyMappingSelect.ActionTriggered += this.ActionKeyMappingSelect;
            Input.Actions.Add(actionKeyMappingSelect);
        }

        public override void UnloadContent()
        {
            characterSpriteFont = null;
            network = null;
            spriteBatch = null;
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void ActionKeyMappingUp(object sender, ActionTriggeredEventArgs e)
        {
            if (characters == null)
                return;

            if (selectedIndex <= 0)
                return;

            selectedIndex--;
        }
        private void ActionKeyMappingSelect(object sender, ActionTriggeredEventArgs e)
        {
            if (characters == null)
                return;

            network.SelectCharacter(characters[selectedIndex].CharacterId);
            GameplayScreen screenToLoad = new GameplayScreen(network);
            LoadingScreen loadingScreen = new LoadingScreen(screenToLoad);
            loadingScreen.Initialize(this.Manager);
            loadingScreen.Activate();
            this.Close();
        }

        private void ActionKeyMappingDown(object sender, ActionTriggeredEventArgs e)
        {
            if (characters == null)
                return;

            if (selectedIndex >= characters.Count - 1)
                return;

            selectedIndex++;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (characters != null)
            {
                int drawIndex = 0;
                foreach (var selChar in characters)
                {
                    spriteBatch.DrawString(characterSpriteFont, selChar.Name, centerScreen.ToVector2() + (fontSizeVector * drawIndex), drawIndex != selectedIndex ? Color.DimGray : Color.White);
                    drawIndex++;
                }
            }
            spriteBatch.End();
        }
    }
}
