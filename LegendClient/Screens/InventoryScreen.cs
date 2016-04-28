using Engine.ScreenEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.InputEngine;
using Microsoft.Xna.Framework.Input;
using LegendWorld.Data.Items;
using LegendWorld.Data;

namespace LegendClient.Screens
{
    internal class InventoryScreen : Screen
    {
        private Texture2D bagTexture;
        private SpriteBatch spriteBatch;
        private Texture2D whitePixel;
        private SpriteFont itemSpriteFont;
        private Texture2D selectionTexture;

        public ClientBagItem BaseContainer { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(whitePixel, this.Game.GraphicsDevice.Viewport.Bounds, this.Game.GraphicsDevice.Viewport.Bounds, Color.Black * .8f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(bagTexture, this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2(), bagTexture.Bounds, Color.White, 0f, bagTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);
            Vector2 drawPosition = this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2() - bagTexture.Bounds.Center.ToVector2();
            if (this.BaseContainer.Items.Count > 0)
            {
                foreach (Item bagItem in this.BaseContainer.ItemsInBag)
                {
                    spriteBatch.DrawString(itemSpriteFont, bagItem.Identity.ToString(), drawPosition, Color.White);
                    drawPosition.Y += itemSpriteFont.LineSpacing;
                }
            }
            else
            {
                spriteBatch.DrawString(itemSpriteFont, "Bag is Empty.", drawPosition, Color.White);
            }

            spriteBatch.Draw(selectionTexture, this.Manager.InputManager.MousePosition.ToVector2(), selectionTexture.Bounds, Color.White, 0f, selectionTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            whitePixel = Game.Content.Load<Texture2D>("WhitePixel");
            itemSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            bagTexture = Game.Content.Load<Texture2D>("CharacterBag");
            selectionTexture = Game.Content.Load<Texture2D>("Selection");

            ActionKeyMapping actionKeyMappingOpenBags = new ActionKeyMapping();
            actionKeyMappingOpenBags.Id = 4;
            actionKeyMappingOpenBags.Primary = Keys.B;
            actionKeyMappingOpenBags.ActionTriggered += ActionKeyMappingOpenBags_ActionTriggered;
            Input.Actions.Add(actionKeyMappingOpenBags);
        }

        private void ActionKeyMappingOpenBags_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            this.Hide();
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
