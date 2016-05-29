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
using LegendClient.World.Items;
using WindowsClient.World.Mobiles;
using LegendWorld.Data.Modifiers;

namespace LegendClient.Screens
{
    internal class CharacterScreen : Screen
    {
        private SpriteBatch spriteBatch;
        private Texture2D whitePixel;
        private SpriteFont powerSpriteFont;
        private Texture2D selectionTexture;

        private int currentIndex = 0;

        public ClientCharacter Character { get; set; }
        public bool IsPlayer { get; set; }

        public CharacterScreen()
        {
        }
        public override void Initialize(ScreenManager screenManager)
        {
            base.Initialize(screenManager);
            ActionKeyMapping actionKeyMappingClose = new ActionKeyMapping();
            actionKeyMappingClose.Id = 4;
            actionKeyMappingClose.Primary = Keys.C;
            actionKeyMappingClose.ActionTriggered += ActionKeyMappingClose_ActionTriggered;
            Input.Actions.Add(actionKeyMappingClose);
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
            ActionKeyMapping actionKeyMappingLeft = new ActionKeyMapping();
            actionKeyMappingLeft.Id = 4;
            actionKeyMappingLeft.Primary = Keys.Left;
            actionKeyMappingLeft.ActionTriggered += ActionKeyMappingLeft_ActionTriggered;
            Input.Actions.Add(actionKeyMappingLeft);
            ActionKeyMapping actionKeyMappingRight = new ActionKeyMapping();
            actionKeyMappingRight.Id = 5;
            actionKeyMappingRight.Primary = Keys.Right;
            actionKeyMappingRight.ActionTriggered += ActionKeyMappingRight_ActionTriggered;
            Input.Actions.Add(actionKeyMappingRight);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(whitePixel, this.Game.GraphicsDevice.Viewport.Bounds, this.Game.GraphicsDevice.Viewport.Bounds, Color.Black * .8f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            //spriteBatch.Draw(bagTexture, this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2(), bagTexture.Bounds, Color.White, 0f, bagTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);

            if (this.Character != null)
            {
                Vector2 drawPosition = this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
                if (this.Character.Powers.Count > 0)
                {
                    int i = 0;
                    foreach (var power in this.Character.Powers)
                    {
                        Color color = Color.White;
                        if (i == currentIndex)
                            color = Color.Red;

                        string listText = power.ToString();

                        spriteBatch.DrawString(powerSpriteFont, listText, drawPosition, color);
                        drawPosition.Y += powerSpriteFont.LineSpacing;
                        i++;
                    }
                }
                else
                {
                    spriteBatch.DrawString(powerSpriteFont, "No Powers.", drawPosition, Color.White);
                }

                drawPosition = Vector2.Zero;
                foreach (CharacterModifier mod in this.Character.Stats.Modifiers)
                {
                    string listText = mod.ToString();

                    spriteBatch.DrawString(powerSpriteFont, listText, drawPosition, Color.White);
                    drawPosition.Y += powerSpriteFont.LineSpacing;
                }

                drawPosition.Y += 100f;
                drawPosition.X += 50f;
                //drawPosition = new Vector2(this.Game.GraphicsDevice.Viewport.Bounds.Width - 400, 0f);
                string test = this.Character.Stats.ToString();
                //foreach (var stat in Stats.All)
                //{
                //    string listText = this.Character.Stats.G.ToString();

                spriteBatch.DrawString(powerSpriteFont, test, drawPosition, Color.White);
                //drawPosition.Y += powerSpriteFont.LineSpacing;
                //}
            }

            spriteBatch.Draw(selectionTexture, this.Manager.InputManager.MousePosition.ToVector2(), selectionTexture.Bounds, Color.White, 0f, selectionTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            whitePixel = Game.Content.Load<Texture2D>("WhitePixel");
            powerSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            //bagTexture = Game.Content.Load<Texture2D>("CharacterBag");
            selectionTexture = Game.Content.Load<Texture2D>("Selection");
        }

        private void ActionKeyMappingLeft_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            //currentIndex = 0;
            //isNavigatingBag = false;
        }

        private void ActionKeyMappingRight_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            //currentIndex = 0;
            //isNavigatingBag = true;
        }

        private void ActionKeyMappingUse_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            //if (isNavigatingBag)
            //{
            //    if (currentIndex < this.BaseContainer.Items.Count)
            //    {
            //        var item = this.BaseContainer.Items[currentIndex];
            //        if (item != null)
            //            this.Use(item, false);

            //        if (currentIndex >= this.BaseContainer.Items.Count && currentIndex > 0)
            //            currentIndex--;
            //    }
            //}
            //else
            //{
            //    if (currentIndex < this.GroundItems.Count)
            //    {
            //        var item = this.GroundItems[currentIndex];
            //        if (item != null)
            //            this.Use(item, true);

            //        if (currentIndex >= this.GroundItems.Count && currentIndex > 0)
            //            currentIndex--;
            //    }
            //}
        }

        //public event EventHandler<ItemUsedEventArgs> ItemUsed;
        //private void Use(IItem item, bool isWorldItem)
        //{
        //    if (this.ItemUsed != null)
        //    {
        //        this.ItemUsed(this, new ItemUsedEventArgs(item, isWorldItem));
        //    }
        //}

        private void ActionKeyMappingUp_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (currentIndex > 0)
                currentIndex--;
        }

        private void ActionKeyMappingDown_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (this.Character.Powers.Count > currentIndex + 1)
                currentIndex++;
        }

        private void ActionKeyMappingClose_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            this.Close();
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
