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

        private int currentItemIndex = 0;

        public ClientBagItem BaseContainer { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(whitePixel, this.Game.GraphicsDevice.Viewport.Bounds, this.Game.GraphicsDevice.Viewport.Bounds, Color.Black * .8f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(bagTexture, this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2(), bagTexture.Bounds, Color.White, 0f, bagTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);
            Vector2 drawPosition = this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2() - bagTexture.Bounds.Center.ToVector2();
            if (this.BaseContainer.Items.Count > 0)
            {
                int i = 0;
                foreach (Item bagItem in this.BaseContainer.ItemsInBag)
                {
                    Color color = Color.White;
                    if (i == currentItemIndex)
                        color = Color.Red;

                    spriteBatch.DrawString(itemSpriteFont, bagItem.Identity.ToString(), drawPosition, color);
                    drawPosition.Y += itemSpriteFont.LineSpacing;
                    i++;
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


            ActionKeyMapping actionKeyMappingUp = new ActionKeyMapping();
            actionKeyMappingUp.Id = 1;
            actionKeyMappingUp.Primary = Keys.Up;
            actionKeyMappingUp.ActionTriggered += ActionKeyMappingUp_ActionTriggered;
            Input.Actions.Add(actionKeyMappingUp);
            ActionKeyMapping actionKeyMappingDown = new ActionKeyMapping();
            actionKeyMappingDown.Id = 2;
            actionKeyMappingDown.Primary = Keys.Down;
            actionKeyMappingDown.ActionTriggered += ActionKeyMappingDown_ActionTriggered;
            Input.Actions.Add(actionKeyMappingDown);
            ActionKeyMapping actionKeyMappingUse = new ActionKeyMapping();
            actionKeyMappingUse.Id = 3;
            actionKeyMappingUse.Primary = Keys.Enter;
            actionKeyMappingUse.ActionTriggered += ActionKeyMappingUse_ActionTriggered;
            Input.Actions.Add(actionKeyMappingUse);
        }

        private void ActionKeyMappingUse_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
           var item = this.BaseContainer.ItemsInBag[currentItemIndex];
            if (item != null)
                this.Use(item);
        }

        public event EventHandler<ItemUsedEventArgs> ItemUsed;
        private void Use(Item item)
        {
            if (this.ItemUsed != null)
            {
                this.ItemUsed(this, new ItemUsedEventArgs(item));
            }
        }

        private void ActionKeyMappingUp_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (this.BaseContainer.ItemsInBag.Count > currentItemIndex + 1)
                currentItemIndex++;
        }

        private void ActionKeyMappingDown_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (currentItemIndex > 0)
                currentItemIndex--;
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
