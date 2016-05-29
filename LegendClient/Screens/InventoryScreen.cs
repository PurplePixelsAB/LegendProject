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
using WindowsClient.Net;
using WindowsClient.World;

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
        private bool isNavigatingBag = true;

        private BagClientItem BaseContainer { get; set; }
        private List<IItem> GroundItems { get; set; }
        //public ClientCharacter Player { get; set; }
        private ClientWorldState world;

        public InventoryScreen(ClientWorldState clientWorldState)
        {
            world = clientWorldState;
            this.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
            this.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory;
        }
        public override void Initialize(ScreenManager screenManager)
        {
            base.Initialize(screenManager);
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
            ActionKeyMapping actionKeyMappingDrop = new ActionKeyMapping();
            actionKeyMappingDrop.Id = 6;
            actionKeyMappingDrop.Primary = Keys.D;
            actionKeyMappingDrop.ActionTriggered += ActionKeyMappingDrop_ActionTriggered;
            Input.Actions.Add(actionKeyMappingDrop);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(whitePixel, this.Game.GraphicsDevice.Viewport.Bounds, this.Game.GraphicsDevice.Viewport.Bounds, Color.Black * .8f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(bagTexture, this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2(), bagTexture.Bounds, Color.White, 0f, bagTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);

            if (this.BaseContainer != null && this.BaseContainer.Items != null)
            {
                Vector2 drawPosition = this.Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2() - bagTexture.Bounds.Center.ToVector2();
                if (this.BaseContainer.Items.Count > 0)
                {
                    int i = 0;
                    foreach (IItem bagItem in this.BaseContainer.Items)
                    {
                        Color color = Color.White;
                        if (i == currentItemIndex && isNavigatingBag)
                            color = Color.Red;

                        string listText = bagItem.ToString();
                        if (world.PlayerCharacter != null)
                        {
                            if (world.PlayerCharacter.IsEquiped(bagItem))
                                listText += " (Equiped)";
                        }

                        spriteBatch.DrawString(itemSpriteFont, listText, drawPosition, color);
                        drawPosition.Y += itemSpriteFont.LineSpacing;
                        i++;
                    }
                }
                else
                {
                    spriteBatch.DrawString(itemSpriteFont, "Bag is Empty.", drawPosition, Color.White);
                }
            }

            if (this.GroundItems != null)
            {
                Vector2 drawPosition = new Vector2(100f, this.Game.GraphicsDevice.Viewport.Bounds.Center.Y);
                spriteBatch.DrawString(itemSpriteFont, "Items on Ground", drawPosition - new Vector2(0, itemSpriteFont.LineSpacing), Color.White);
                if (this.GroundItems.Count > 0)
                {
                    int i = 0;
                    foreach (IClientItem groundItem in this.GroundItems)
                    {
                        Color color = Color.White;
                        if (i == currentItemIndex && !isNavigatingBag)
                            color = Color.Red;

                        spriteBatch.DrawString(itemSpriteFont, groundItem.ToString(), drawPosition, color);
                        drawPosition.Y += itemSpriteFont.LineSpacing;
                        i++;
                    }
                }
                else
                {
                    spriteBatch.DrawString(itemSpriteFont, "No Items close by.", drawPosition, Color.White);
                }
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
        }

        private void ActionKeyMappingLeft_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            currentItemIndex = 0;
            isNavigatingBag = false;
        }

        private void ActionKeyMappingRight_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            currentItemIndex = 0;
            isNavigatingBag = true;
        }

        private void ActionKeyMappingUse_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (isNavigatingBag)
            {
                if (currentItemIndex < this.BaseContainer.Items.Count)
                {
                    var item = this.BaseContainer.Items[currentItemIndex];
                    if (item != null)
                        this.Use(item, false);

                    if (currentItemIndex >= this.BaseContainer.Items.Count && currentItemIndex > 0)
                        currentItemIndex--;
                }
            }
            else
            {
                if (currentItemIndex < this.GroundItems.Count)
                {
                    var item = this.GroundItems[currentItemIndex];
                    if (item != null)
                        this.Use(item, true);

                    if (currentItemIndex >= this.GroundItems.Count && currentItemIndex > 0)
                        currentItemIndex--;
                }
            }
        }

        private void ActionKeyMappingDrop_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (isNavigatingBag)
            {
                if (currentItemIndex < this.BaseContainer.Items.Count)
                {
                    var item = this.BaseContainer.Items[currentItemIndex];
                    if (item != null)
                    {
                        if (world.PlayerCharacter.DropItem(item))
                        {
                            NetworkEngine.Instance.PickUpItem(world.PlayerCharacter.Id, item);
                            this.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(this.Player.InventoryBagId));
                            this.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
                        }
                    }

                    if (currentItemIndex >= this.BaseContainer.Items.Count && currentItemIndex > 0)
                        currentItemIndex--;
                }
            }
        }
        //public event EventHandler<ItemUsedEventArgs> ItemUsed;
        private void Use(IItem item, bool isWorldItem)
        {
            //if (this.ItemUsed != null)
            //{
            //    this.ItemUsed(this, new ItemUsedEventArgs(item, isWorldItem));
            //}            
            if (isWorldItem)
            {
                if (world.PlayerCharacter.PickupItem(item))
                {
                    NetworkEngine.Instance.PickUpItem(world.PlayerCharacter.Id, item);
                    this.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(this.Player.InventoryBagId));
                    this.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
                }
            }
            else
            {
                if (item.Category == ItemCategory.Consumable)
                {
                    ConsumableItem consumable = (ConsumableItem)item;
                    if (consumable.Use(world.PlayerCharacter, world))
                    {
                        NetworkEngine.Instance.UseItem(world.PlayerCharacter.Id, consumable);
                        this.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(this.Player.InventoryBagId));
                        this.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
                    }
                }
                if (item.Category == ItemCategory.Armor || item.Category == ItemCategory.Weapon)
                {
                    if (world.PlayerCharacter.Equip(item))
                    {
                        NetworkEngine.Instance.UseItem(world.PlayerCharacter.Id, item);
                        this.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(this.Player.InventoryBagId));
                        this.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
                    }
                }
            }
        }

        private void ActionKeyMappingUp_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (currentItemIndex > 0)
                currentItemIndex--;
        }

        private void ActionKeyMappingDown_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (isNavigatingBag)
            {
                if (this.BaseContainer.Items.Count > currentItemIndex + 1)
                    currentItemIndex++;
            }
            else
            {
                if (this.GroundItems.Count > currentItemIndex + 1)
                    currentItemIndex++;
            }
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
