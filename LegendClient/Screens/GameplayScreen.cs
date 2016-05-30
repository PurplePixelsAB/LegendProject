using Engine.ScreenEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Data.World;
using Engine.InputEngine;
using Microsoft.Xna.Framework.Input;
using WindowsClient.Net;
using System.Threading;
using WindowsClient.World.Mobiles;
using Network;
using WindowsClient.World;
using LegendClient.Screens;
using LegendWorld.Data.Items;
using LegendWorld.Data;
using LegendClient.World.Items;
using Data;
using LegendClient.Effects;
using LegendClient.World.Mobiles;

namespace WindowsClient
{
    public class GameplayScreen : Screen
    {
        public bool IsConnected { get { return this.network.ConnectedToWorld; } }

        private SpriteBatch spriteBatch;
        private Texture2D bodyTexture;
        private Texture2D headTexture;
        //private Texture2D leatherHoodTexture;
        //private Texture2D leatherArmorTexture;
        //private Texture2D bowTexture;
        private Texture2D backgroundTexture;
        private MovementBodyBobEffect movementBodyBobEffect = new MovementBodyBobEffect();
        //private InventoryScreen inventoryScreen;
        private Texture2D bodyGhostTexture;
        private Texture2D headGhostTexture;

        private Queue<DamageTextEffect> DamageTextEffectList = new Queue<DamageTextEffect>();
        private Texture2D bigBushTexture;

        private NetworkEngine network;
        private ClientWorldState world;
        private WorldPump worldPump;
        private EffectManager effectManager;
        private Texture2D selectionTexture;
        private SpriteFont damageSpriteFont;

        //ActionKeyMapping[] moveMappings;
        //ActionKeyMapping[] generalMappings;
        Rectangle Viewport;
        Vector2 CenterScreenVector2; //ToDo: Remove
        Point centerScreen;

        private long lastMovementClickTicks;
        private TimeSpan doubleClickSpeed = new TimeSpan(0, 0, 0, 0, 200);
        private Texture2D hudbarTexture;
        private Texture2D arrowTexture;
        private Texture2D chatIcon;
        private SpriteFont chatSpriteFont;
        private Effect deadEffect;

        public GameplayScreen()
        {
            Viewport = new Rectangle(0, 0, 1920, 1080);
            centerScreen = Viewport.Center;
            CenterScreenVector2 = centerScreen.ToVector2();

            world = new ClientWorldState();
            world.CharacterAdded += World_CharacterAdded;
            network = NetworkEngine.Instance; //networkEngine;
            network.WorldState = world;
            worldPump = new WorldPump();
            worldPump.State = world;

            effectManager = new EffectManager();
            effectManager.UseDayNightCycle = true;
            effectManager.DayColor = Color.White;
            effectManager.NightColor = new Color(.2f, .2f, .4f);//Color.DarkBlue;

            //inventoryScreen = new InventoryScreen();
            //inventoryScreen.ItemUsed += InventoryScreen_ItemUsed;
            //inventoryScreen.Player = world.PlayerCharacter;
        }

        public override void Initialize(ScreenManager screenManager)
        {
            base.Initialize(screenManager);
            //inventoryScreen.Initialize(screenManager);
            network.Initialize();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.PowerScoll,
                new ClientItemFactory<PowerScrollClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Scroll") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Gold,
                new ClientItemFactory<GoldClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Gold") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Bandage,
                new ClientItemFactory<BandageClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Bandage") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Bag,

                new ClientItemFactory<BagClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Bag") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Bow,
                new ClientItemFactory<BowClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Bow") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Sword,
                new ClientItemFactory<SwordClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Sword") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.Dagger,
                new ClientItemFactory<DaggerClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/Dagger") });

            ClientItemFactory.Load(Data.ItemData.ItemIdentity.LeatherArmor,
                new ArmorClientItemFactory<LeatherArmorClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/LeatherArmor"), HeadTexture = Game.Content.Load<Texture2D>("Items/LeatherHead") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.PlateArmor,
                new ArmorClientItemFactory<PlateArmorClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/PlateArmor"), HeadTexture = Game.Content.Load<Texture2D>("Items/PlateHead") });
            ClientItemFactory.Load(Data.ItemData.ItemIdentity.ClothRobe,
                new ArmorClientItemFactory<ClothClientItem>() { Texture = Game.Content.Load<Texture2D>("Items/ClothArmor"), HeadTexture = Game.Content.Load<Texture2D>("Items/ClothHead") });

            if (!network.LoadContent(world))
            {
                this.Disconnect("Failed to load world.");
                return;
            }
            //inventoryScreen.LoadContent(graphicsDevice);

            spriteBatch = new SpriteBatch(graphicsDevice);
            bodyTexture = Game.Content.Load<Texture2D>("Body");
            headTexture = Game.Content.Load<Texture2D>("Head");
            bodyGhostTexture = Game.Content.Load<Texture2D>("GhostBody");
            headGhostTexture = Game.Content.Load<Texture2D>("GhostHead");
            //leatherHoodTexture = Game.Content.Load<Texture2D>("LeatherHood");
            //leatherArmorTexture = Game.Content.Load<Texture2D>("LetherArmor");
            //bowTexture = Game.Content.Load<Texture2D>("BowSmall");
            backgroundTexture = Game.Content.Load<Texture2D>("GrassBackground");
            selectionTexture = Game.Content.Load<Texture2D>("Selection");
            damageSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            chatSpriteFont = Game.Content.Load<SpriteFont>("ChatFont");
            bigBushTexture = Game.Content.Load<Texture2D>("bigbush");
            hudbarTexture = Game.Content.Load<Texture2D>("HudBar");
            arrowTexture = Game.Content.Load<Texture2D>("ArrowProjectile");
            chatIcon = Game.Content.Load<Texture2D>("ChatIcon");
            deadEffect = Game.Content.Load<Effect>("DeadEffect");

            effectManager.LoadContent(graphicsDevice, Game.Content);

            //generalMappings = Game.Content.Load<ActionKeyMapping[]>("DefaultKeys\\General");
            //for (int i = 0; i <= generalMappings.GetUpperBound(0); i++)
            //{
            //    generalMappings[i].ActionTriggered += new ActionTriggeredEventHandler(GeneralKeys_ActionTriggered);
            //}
            //Input.Actions.AddRange(generalMappings);


            //DateTime timeOut = DateTime.Now.AddSeconds(20);
            //while (!network.Connected && DateTime.Now < timeOut)
            //{
            //    Thread.Sleep(500);
            //}
            //if (!network.Connected)
            //{
            //    this.Game.Exit();
            //    return;
            //}

            ActionButtonMapping actionButtonMappingMoveTo = new ActionButtonMapping();
            actionButtonMappingMoveTo.Id = 1;
            actionButtonMappingMoveTo.Primary = MouseButtons.Right;
            actionButtonMappingMoveTo.ActionTriggered += ActionButtonMappingMoveTo_ActionTriggered;
            Input.Actions.Add(actionButtonMappingMoveTo);

            ActionButtonMapping actionButtonMappingAimTo = new ActionButtonMapping();
            actionButtonMappingAimTo.Id = 2;
            actionButtonMappingAimTo.Primary = MouseButtons.Left;
            actionButtonMappingAimTo.ActionTriggered += ActionButtonMappingAimTo_ActionTriggered;
            Input.Actions.Add(actionButtonMappingAimTo);

            ActionKeyMapping actionKeyMappingAbility1 = new ActionKeyMapping();
            actionKeyMappingAbility1.Id = 11;
            actionKeyMappingAbility1.Primary = Keys.D1;
            actionKeyMappingAbility1.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility1);
            ActionKeyMapping actionKeyMappingAbility2 = new ActionKeyMapping();
            actionKeyMappingAbility2.Id = 12;
            actionKeyMappingAbility2.Primary = Keys.D2;
            actionKeyMappingAbility2.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility2);
            ActionKeyMapping actionKeyMappingAbility3 = new ActionKeyMapping();
            actionKeyMappingAbility3.Id = 13;
            actionKeyMappingAbility3.Primary = Keys.D3;
            actionKeyMappingAbility3.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility3);
            ActionKeyMapping actionKeyMappingAbility4 = new ActionKeyMapping();
            actionKeyMappingAbility4.Id = 14;
            actionKeyMappingAbility4.Primary = Keys.D4;
            actionKeyMappingAbility4.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility4);
            ActionKeyMapping actionKeyMappingAbility5 = new ActionKeyMapping();
            actionKeyMappingAbility5.Id = 15;
            actionKeyMappingAbility5.Primary = Keys.D5;
            actionKeyMappingAbility5.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility5);

            ActionKeyMapping actionKeyMappingOpenBags = new ActionKeyMapping();
            actionKeyMappingOpenBags.Id = 4;
            actionKeyMappingOpenBags.Primary = Keys.B;
            actionKeyMappingOpenBags.ActionTriggered += ActionKeyMappingOpenBags_ActionTriggered;
            Input.Actions.Add(actionKeyMappingOpenBags);
            ActionKeyMapping actionKeyMappingCharacter = new ActionKeyMapping();
            actionKeyMappingCharacter.Id = 5;
            actionKeyMappingCharacter.Primary = Keys.C;
            actionKeyMappingCharacter.ActionTriggered += ActionKeyMappingOpenCharacter_ActionTriggered;
            Input.Actions.Add(actionKeyMappingCharacter);

            ActionKeyMapping actionKeyMappingChat = new ActionKeyMapping();
            actionKeyMappingChat.Id = 6;
            actionKeyMappingChat.Primary = Keys.Enter;
            actionKeyMappingChat.ActionTriggered += ActionKeyMappingChat_ActionTriggered;
            Input.Actions.Add(actionKeyMappingChat);

            ActionKeyMapping actionKeyMappingToggleFlullscreen = new ActionKeyMapping();
            actionKeyMappingToggleFlullscreen.Id = 0;
            actionKeyMappingToggleFlullscreen.Primary = Keys.Enter;
            actionKeyMappingToggleFlullscreen.PrimaryMod = Keys.LeftControl;
            actionKeyMappingToggleFlullscreen.ActionTriggered += ActionKeyMappingToggleFlullscreen_ActionTriggered;
            Input.Actions.Add(actionKeyMappingToggleFlullscreen);

            ActionKeyMapping actionKeyMappingGameMenu = new ActionKeyMapping();
            actionKeyMappingGameMenu.Id = 42;
            actionKeyMappingGameMenu.Primary = Keys.Escape;
            actionKeyMappingGameMenu.ActionTriggered += this.ActionKeyMappingOpenGameMenu;
            Input.Actions.Add(actionKeyMappingGameMenu);
        }

        private void ActionKeyMappingChat_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            ChatScreen chatScreen = new ChatScreen(world.PlayerCharacter.Id);
            chatScreen.Initialize(this.Manager);
            chatScreen.Show();
        }

        private void ActionKeyMappingOpenGameMenu(object sender, ActionTriggeredEventArgs e)
        {
            GameMenuScreen menuScreen = new GameMenuScreen();
            menuScreen.Initialize(this.Manager);
            menuScreen.Show();
        }
        private void ActionKeyMappingOpenCharacter_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (world.PlayerCharacter == null)
                return;
            if (world.PlayerCharacter.Powers == null)
                return;
            //if (world.PlayerCharacter.IsDead)
            //    return;

            CharacterScreen characterScreen = new CharacterScreen();
            characterScreen.Character = world.PlayerCharacter;
            characterScreen.Initialize(this.Manager);
            characterScreen.Activate();

            //inventoryScreen.Player = world.PlayerCharacter;
            //inventoryScreen.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(world.PlayerCharacter.InventoryBagId));
            //inventoryScreen.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
            //inventoryScreen.Activate();
        }
        private void ActionKeyMappingOpenBags_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            if (world.PlayerCharacter == null)
                return;
            if (world.PlayerCharacter.Inventory == null)
                return;
            if (world.PlayerCharacter.IsDead)
                return;
            InventoryScreen inventoryScreen = new InventoryScreen(world);
            //inventoryScreen.Player = world.PlayerCharacter;
            //inventoryScreen.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(world.PlayerCharacter.InventoryBagId));
            //inventoryScreen.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
            inventoryScreen.Initialize(this.Manager);
            inventoryScreen.Activate();
        }
        //private void InventoryScreen_ItemUsed(object sender, ItemUsedEventArgs e)
        //{
        //    if (e.IsWorldItem)
        //    {
        //        if (world.PlayerCharacter.PickupItem(e.ItemUsed))
        //        {
        //            network.UseItem(world.PlayerCharacter.Id, e.ItemUsed);
        //            inventoryScreen.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
        //        }
        //    }
        //    else
        //    {
        //        if (e.ItemUsed.Category == ItemCategory.Consumable)
        //        {
        //            ConsumableItem consumable = (ConsumableItem)e.ItemUsed;
        //            if (consumable.Use(world.PlayerCharacter, world))
        //            {
        //                network.UseItem(world.PlayerCharacter.Id, consumable);
        //                inventoryScreen.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(world.PlayerCharacter.InventoryBagId));
        //                inventoryScreen.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
        //            }
        //        }
        //        if (e.ItemUsed.Category == ItemCategory.Armor || e.ItemUsed.Category == ItemCategory.Weapon)
        //        {
        //            if (world.PlayerCharacter.Equip(e.ItemUsed))
        //            {
        //                network.UseItem(world.PlayerCharacter.Id, e.ItemUsed);
        //                inventoryScreen.BaseContainer = (BagClientItem)world.PlayerCharacter.Inventory; //new ClientBagItem((BagItem)world.GetItem(world.PlayerCharacter.InventoryBagId));
        //                inventoryScreen.GroundItems = world.GroundItemsInRange(world.PlayerCharacter.Id);
        //            }
        //        }
        //    }
        //}

        private void ActionKeyMappingToggleFlullscreen_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            this.Game.Window.IsBorderless = !this.Game.Window.IsBorderless;
            this.Game.Window.Position = Point.Zero;
        }

        private void World_CharacterAdded(object sender, NewCharacterEventArgs e)
        {
            e.Character.Stats.OnStatChangedRegister(StatIdentifier.Health, Character_HealthChanged);
            //e.Character.HealthChanged += Character_HealthChanged;
            e.Character.PerformsPower += Character_PerformsPower;
            e.Character.AffectedByPower += Character_AffectedByPower;
        }

        private void Character_AffectedByPower(object sender, AffectedByPowerEventArgs e)
        {
            if (sender == null)
                return;

            CharacterPower power = e.CharacterPower;
            ClientCharacter character = (ClientCharacter)sender;
        }

        private void Character_PerformsPower(object sender, PerformsPowerEventArgs e)
        {
            if (sender == null)
                return;

            CharacterPower power = e.CharacterPower;
            ClientCharacter character = (ClientCharacter)sender;

            switch (power.Id)
            {
                case CharacterPowerIdentity.DefaultAttack:
                    effectManager.AddEffect(new SwingEffect(this.GetScreenPostion(character.DrawPosition), this.GetScreenPostion(character.AimToPosition), character.IsEquiped(ItemData.ItemIdentity.Bow)));
                    break;
                case CharacterPowerIdentity.HardAttack:
                    effectManager.AddEffect(new SwingEffect(this.GetScreenPostion(character.DrawPosition), this.GetScreenPostion(character.AimToPosition), character.IsEquiped(ItemData.ItemIdentity.Bow)));
                    break;
                case CharacterPowerIdentity.CriticalAttack:
                    break;
                case CharacterPowerIdentity.StunAttack:
                    break;
                case CharacterPowerIdentity.SlowingAttack:
                    break;
                case CharacterPowerIdentity.DecreaseEnergyCost:
                    break;
                case CharacterPowerIdentity.IncreaseEnergyCost:
                    break;
                case CharacterPowerIdentity.DecreaseDuration:
                    break;
                case CharacterPowerIdentity.IncreaseDuration:
                    break;
                case CharacterPowerIdentity.Meditation:
                    break;
                case CharacterPowerIdentity.AbsorbDamage:
                    break;
                case CharacterPowerIdentity.DeflectDamage:
                    break;
                case CharacterPowerIdentity.ShortSpeedBurst:
                    break;
                case CharacterPowerIdentity.IncreaseSpeed:
                    break;
                case CharacterPowerIdentity.IncreaseMaxHealth:
                    break;
                case CharacterPowerIdentity.IncreaseMaxEnergy:
                    break;
                case CharacterPowerIdentity.IncreaseHealthRegen:
                    break;
                case CharacterPowerIdentity.IncreaseEnergyRegen:
                    break;
                case CharacterPowerIdentity.Stealth:
                    break;
                case CharacterPowerIdentity.IncreaseSwordPower:
                    break;
                case CharacterPowerIdentity.IncreaseBowPower:
                    break;
                case CharacterPowerIdentity.IncreasePlateArmor:
                    break;
                case CharacterPowerIdentity.IncreaseLeatherArmor:
                    break;
                case CharacterPowerIdentity.IncreaseMaxWeight:
                    break;
                case CharacterPowerIdentity.Interrupt:
                    break;
                default:
                    break;
            }
        }

        private void Character_HealthChanged(Character character, StatChangedEventArgs e)
        {
            ClientCharacter clientCharacter = (ClientCharacter)character;
            if (e.Value <= e.PreviousValue) //Damage
            {
                this.AddDamageIndicator(clientCharacter, e.PreviousValue - e.Value);
                effectManager.AddEffect(new BloodEffect(this.GetScreenPostion(clientCharacter.Position)));
            }
            else //Healing
            {
                this.AddHealIndicator(clientCharacter, e.Value - e.PreviousValue);
            }
        }

        private void AddHealIndicator(ClientCharacter clientCharacter, int healAmount)
        {
            var dmgEfx = new DamageTextEffect();
            dmgEfx.Character = clientCharacter;
            dmgEfx.Text = string.Format("{0}", healAmount);
            dmgEfx.Duration = 2000;
            dmgEfx.Color = Color.White;
            DamageTextEffectList.Enqueue(dmgEfx);
        }

        private void AddDamageIndicator(ClientCharacter clientCharacter, int damageAmount)
        {
            var dmgEfx = new DamageTextEffect();
            dmgEfx.Character = clientCharacter;
            dmgEfx.Text = string.Format("{0}", damageAmount);
            dmgEfx.Duration = 2000;
            dmgEfx.Color = Color.Red;
            DamageTextEffectList.Enqueue(dmgEfx);
        }

        private void actionKeyMappingAbility_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            var abilityIndex = e.Action.Id - 12;

            if (abilityIndex == -1)
            {
                if (world.PerformAbility(CharacterPowerIdentity.DefaultAttack, world.PlayerCharacter))
                {
                    network.PerformAbility(world.PlayerCharacter, CharacterPowerIdentity.DefaultAttack);
                }
            }
            else
            {
                if (world.PlayerCharacter.Powers.Count > abilityIndex)
                {
                    CharacterPowerIdentity abilityId = world.PlayerCharacter.Powers[abilityIndex];

                    if (world.PerformAbility(abilityId, world.PlayerCharacter))
                    {
                        network.PerformAbility(world.PlayerCharacter, abilityId);
                    }
                }
            }
        }

        private void ActionButtonMappingAimTo_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            Point worldPosition = ScreenToWorld(e.MouseState.Position);
            if (world.PlayerCharacter.AimToPosition != worldPosition)
            {
                world.PlayerCharacter.SetAimToPosition(worldPosition);
                network.AimTo(world.PlayerCharacter.Id, worldPosition);
            }
        }


        private void ActionButtonMappingMoveTo_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            Point worldPosition = ScreenToWorld(e.MouseState.Position);

            if (world.PlayerCharacter.MovingToPosition != worldPosition)
            {
                world.PlayerCharacter.SetMoveToPosition(worldPosition);
                network.MoveTo(world.PlayerCharacter.Id, worldPosition);
            }

            if (lastMovementClickTicks + doubleClickSpeed.Ticks >= e.GameTime.TotalGameTime.Ticks)
            {
                if (world.PlayerCharacter.AimToPosition != worldPosition)
                {
                    world.PlayerCharacter.SetAimToPosition(worldPosition);
                    network.AimTo(world.PlayerCharacter.Id, worldPosition);
                }
            }
            lastMovementClickTicks = e.GameTime.TotalGameTime.Ticks;
        }

        private Point ScreenToWorld(Point mousePoint)
        {
            Point screenCenter = new Point(960, 540);
            return new Point(world.PlayerCharacter.Position.X + (mousePoint.X - screenCenter.X), world.PlayerCharacter.Position.Y + (mousePoint.Y - screenCenter.Y));
        }

        public override void UnloadContent()
        {
            network.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            network.Update(gameTime);
            if (!this.IsConnected)
            {
                this.Disconnect(string.Empty);
            }

            worldPump.Update(gameTime);
            world.ClientUpdate(gameTime);
            movementBodyBobEffect.Update(gameTime);
            effectManager.Update(gameTime);
        }

        //public void UpdateRandomBodyMovement(GameTime gameTime)
        //{
        //    //ToDo: SAVE CODE - Random Body movement instead.
        //    //if ((lastUpdateTime.TotalSeconds + updateTime) <= gameTime.TotalGameTime.TotalSeconds)
        //    //{
        //    //    rndHeadLast = rndHeadTarget;
        //    //    rndHeadTarget = new Vector2(rnd.Next(-2, 2), rnd.Next(-1, 3));
        //    //    lastUpdateTime = gameTime.TotalGameTime;
        //    //    updateTime = rnd.Next(1, 3);
        //    //}
        //    //float lerp = (float)((gameTime.TotalGameTime.TotalSeconds - lastUpdateTime.TotalSeconds) / updateTime);
        //    //rndHead.X = (float)Math.Round(MathHelper.Lerp(rndHeadLast.X, rndHeadTarget.X, lerp));
        //    //rndHead.Y = (float)Math.Round(MathHelper.Lerp(rndHeadLast.Y, rndHeadTarget.Y, lerp));
        //}

        //private float Vector2ToRadian(Vector2 direction)
        //{
        //    return (float)Math.Atan2(direction.X, -direction.Y) + MathHelper.Pi;
        //}

        public void Disconnect(string messageReson)
        {
            DisconnectedScreen screen = new DisconnectedScreen(messageReson);
            screen.Initialize(this.Manager);
            screen.Activate();
            this.Close();
        }
        public override void Draw(GameTime gameTime)
        {
            if (!this.IsConnected)
                return;

            effectManager.CreateLightMap(Game.GraphicsDevice, spriteBatch, gameTime);

            if (!world.PlayerCharacter.IsDead)
                spriteBatch.Begin();
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, deadEffect);

            this.BaseDrawing(spriteBatch);
            this.DrawGroundItems(spriteBatch);
            this.DrawCharacters(spriteBatch);
            this.DrawProjectiles(spriteBatch);
            spriteBatch.End();

            effectManager.Draw(spriteBatch, gameTime);

            spriteBatch.Begin();
            this.DrawDamageEffect(spriteBatch, gameTime);
            this.DrawHud(spriteBatch, gameTime);
            spriteBatch.End();
        }

        private void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (var arrow in world.Projectiles)
            {
                Vector2 arrowDirection = arrow.Target.ToVector2() - arrow.Position.ToVector2();
                spriteBatch.Draw(arrowTexture, this.GetScreenPostion(arrow.Position).ToVector2(), null, Color.White, (float)world.VectorToRadian(arrowDirection), arrowTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);
            }
        }

        private void DrawGroundItems(SpriteBatch spriteBatch)
        {
            List<IClientItem> items = world.GetItemsOnGround(world.PlayerCharacter.CurrentMapId);
            foreach (IClientItem item in items)
            {
                //GroundItem groundItem = world.GetGroundItem(itemId);

                //ToDo: ClientItem clientItem = (ClientItem)item;
                // clientItem.WorldDrawPosition ...
                // clientItem.Texture ...
                Point drawPosition = this.GetScreenPostion(item.Data.WorldLocation - item.Texture.Bounds.Center);
                //groundItem.Position.ToVector2() - (world.PlayerCharacter.OldDrawPosition * -1f) - itemScrollTexture.Bounds.Center.ToVector2(); 
                //CenterScreen - (world.PlayerCharacter.Position - groundItem.Position).ToVector2();
                spriteBatch.Draw(item.Texture, new Rectangle(drawPosition, item.Texture.Bounds.Size), Color.White);
            }
        }

        private void DrawHud(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.DrawCharacterStatusBar(spriteBatch, gameTime);
            this.DrawCharacterIndicators(spriteBatch, gameTime);
            this.DrawServerMessages(spriteBatch, gameTime);

            foreach (ChatMessage message in world.ChatMessages)
            {
                var textSize = chatSpriteFont.MeasureString(message.Text);
                var halfSize = textSize * .5f;
                spriteBatch.DrawString(chatSpriteFont, message.Text, this.GetScreenPostion(message.Owner.DrawPosition).ToVector2() - halfSize - new Vector2(0f, 30f), Color.White);
            }

            //Draw Mouse Pointer
            spriteBatch.Draw(selectionTexture, Mouse.GetState().Position.ToVector2(), selectionTexture.Bounds, Color.White, 0f, selectionTexture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);
        }

        public void DrawCharacterIndicators(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 centerSelection = selectionTexture.Bounds.Center.ToVector2();
            //Point screenCenter = new Point(960, 540);
            Point drawAimLocation = centerScreen - (world.PlayerCharacter.DrawPosition - world.PlayerCharacter.AimToPosition); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);
            spriteBatch.Draw(selectionTexture, new Rectangle(drawAimLocation, selectionTexture.Bounds.Size), selectionTexture.Bounds, Color.Blue, 0f, centerSelection, SpriteEffects.None, 1f);

            if (world.PlayerCharacter.Position != world.PlayerCharacter.MovingToPosition)
            {
                Point drawMoveToLocation = centerScreen - (world.PlayerCharacter.DrawPosition - world.PlayerCharacter.MovingToPosition); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);
                spriteBatch.Draw(selectionTexture, new Rectangle(drawMoveToLocation, selectionTexture.Bounds.Size), selectionTexture.Bounds, Color.Cyan, 0f, centerSelection, SpriteEffects.None, 1f);
            }
        }
        public void DrawCharacterStatusBar(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (world.PlayerCharacter.IsDead)
                return;

            Rectangle sourceSize = hudbarTexture.Bounds;

            var healthDrawPosition = new Vector2(CenterScreenVector2.X - sourceSize.Center.X, CenterScreenVector2.Y + (CenterScreenVector2.Y * .7f));
            spriteBatch.Draw(hudbarTexture, healthDrawPosition, Color.White);

            int healthBasedWidth = (int)(sourceSize.Width * ((float)world.PlayerCharacter.Stats.Health / (float)world.PlayerCharacter.Stats.MaxHealth));
            Rectangle healthSize = sourceSize;
            healthSize.Width = healthBasedWidth;
            spriteBatch.Draw(hudbarTexture, healthDrawPosition, healthSize, Color.DarkGreen);

            var energyDrawPosition = healthDrawPosition + new Vector2(0f, sourceSize.Size.Y);
            spriteBatch.Draw(hudbarTexture, energyDrawPosition, Color.White);

            int energyBasedWidth = (int)(sourceSize.Width * ((float)world.PlayerCharacter.Stats.Energy / (float)world.PlayerCharacter.Stats.MaxEnergy));
            Rectangle energySize = sourceSize;
            energySize.Width = energyBasedWidth;
            spriteBatch.Draw(hudbarTexture, energyDrawPosition, energySize, Color.Orange);
        }
        private void DrawServerMessages(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var messages = network.GetServerMessages();
            Vector2 drawLocation = new Vector2();
            Vector2 lineDistance = new Vector2(0f, damageSpriteFont.LineSpacing + 2);
            foreach (var msg in messages)
            {
                spriteBatch.DrawString(damageSpriteFont, msg.Message, drawLocation, Color.Red);
                drawLocation += lineDistance;
            }
        }

        private void DrawDamageEffect(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var queue = DamageTextEffectList;
            DamageTextEffectList = new Queue<DamageTextEffect>();
            while (queue.Count > 0)
            {
                var effect = queue.Dequeue();

                Point effectDrawLocation = centerScreen - (world.PlayerCharacter.DrawPosition - effect.Character.DrawPosition) + effect.OffsetPostion; //new Vector2(world.PlayerCharacter.Position.X - charTakingDmgLocation.X, world.PlayerCharacter.Position.Y - charTakingDmgLocation.Y);
                spriteBatch.DrawString(damageSpriteFont, effect.Text, effectDrawLocation.ToVector2(), effect.Color);
                effect.Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
                effect.OffsetPostion -= new Point(0, 1);
                if (effect.Duration > 0D)
                    DamageTextEffectList.Enqueue(effect);
            }
        }



        private Point GetScreenPostion(Point worldPostion)
        {
            return (centerScreen - (world.PlayerCharacter.DrawPosition - worldPostion));
        }

        private void DrawCharacters(SpriteBatch spriteBatch)
        {
            Vector2 centerHead = headTexture.Bounds.Center.ToVector2();
            Vector2 centerBody = bodyTexture.Bounds.Center.ToVector2();
            centerBody.Y = 0f;

            Texture2D bodyTextureToUse, headTextureToUse;

            foreach (ushort id in world.Characters)
            {
                //if (id == world.PlayerCharacter.Id)
                //    continue;
                bodyTextureToUse = bodyTexture;
                headTextureToUse = headTexture;

                ClientCharacter charToDraw = (ClientCharacter)world.GetCharacter(id);
                Vector2 charToDrawDirection = charToDraw.AimToPosition.ToVector2() - charToDraw.Position.ToVector2();
                Vector2 bodyMovingBobPosition = Vector2.Zero;
                if (charToDraw.IsMoving)
                    bodyMovingBobPosition = movementBodyBobEffect.PositioinBob;

                Vector2 bodyRotationPosition = charToDrawDirection;
                if (bodyRotationPosition != Vector2.Zero)
                    bodyRotationPosition.Normalize();
                bodyRotationPosition.X *= 2f;
                if (bodyRotationPosition.Y < 0f)
                    bodyRotationPosition.Y *= 10f;

                if (charToDraw.IsDead)
                {
                    bodyTextureToUse = bodyGhostTexture;
                    headTextureToUse = headGhostTexture;
                }

                var distance = Vector2.Distance(world.PlayerCharacter.Position.ToVector2(), charToDraw.Position.ToVector2());
                float visibility = charToDraw.Stats.GetVisibility(distance);

                if (visibility > 0f)
                {
                    Color visibilityColor = Color.White * visibility;
                    //Draw Body
                    Vector2 clientScreenPostion = this.GetScreenPostion(charToDraw.DrawPosition).ToVector2();
                    spriteBatch.Draw(bodyTextureToUse, clientScreenPostion + bodyMovingBobPosition + bodyRotationPosition, null, visibilityColor,
                        0f, centerBody, 1f, SpriteEffects.None, 1f);
                    if (charToDraw.Armor != null)
                    {
                        IArmorClientItem armorItem = (IArmorClientItem)charToDraw.Armor;
                        spriteBatch.Draw(armorItem.Texture, clientScreenPostion + bodyMovingBobPosition + bodyRotationPosition, null, visibilityColor,
                            0f, new Vector2(armorItem.Texture.Bounds.Center.X, 0f), 1f, SpriteEffects.None, 1f);
                    }

                    //Draw Head
                    spriteBatch.Draw(headTextureToUse, clientScreenPostion, null, visibilityColor,
                        (float)world.VectorToRadian(charToDrawDirection), centerHead, 1f, SpriteEffects.None, 1f);

                    //Draw Armor
                    if (charToDraw.Armor != null)
                    {
                        IArmorClientItem armorItem = (IArmorClientItem)charToDraw.Armor;
                        spriteBatch.Draw(armorItem.HeadTexture, clientScreenPostion, null, visibilityColor,
                            (float)world.VectorToRadian(charToDrawDirection), centerHead, 1f, SpriteEffects.None, 1f);
                    }

                    //Draw Weapons
                    if (charToDraw.RightHand != null)
                    {
                        IClientItem weaponItem = (IClientItem)charToDraw.RightHand;
                        spriteBatch.Draw(weaponItem.Texture, (clientScreenPostion - (bodyMovingBobPosition * .5f)) + bodyRotationPosition + new Vector2(-weaponItem.Texture.Bounds.Width, weaponItem.Texture.Bounds.Center.Y * .5f), null, visibilityColor,
                            -.5f, weaponItem.Texture.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1f);
                    }

                    //Draw ChatIcon
                    if (charToDraw.IsWritingMessage)
                    {
                        spriteBatch.Draw(chatIcon, clientScreenPostion + new Vector2(0f, -30f), null, visibilityColor,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                }
            }

            //Vector2 start = world.PlayerCharacter.Position.ToVector2();
            //Vector2 end = world.PlayerCharacter.AimToPosition.ToVector2();
            ////float distance = Vector2.Distance(start, end);
            //Vector2 direction = end - start;

            //Vector2 plrbodyMovingBobPosition = Vector2.Zero;
            //if (world.PlayerCharacter.IsMoving)
            //    plrbodyMovingBobPosition = movementBodyBobEffect.PositioinBob;

            //Vector2 plrbodyRotationPosition = direction;
            //plrbodyRotationPosition.Normalize();
            //plrbodyRotationPosition.X *= 2f;
            //if (plrbodyRotationPosition.Y < 0f)
            //plrbodyRotationPosition.Y *= 10f;

            //spriteBatch.Draw(bodyTexture, CenterScreenVector2 + plrbodyMovingBobPosition + plrbodyRotationPosition, null, Color.White, 0f, centerBody, 1f, SpriteEffects.None, 1f);
            //spriteBatch.Draw(headTexture, CenterScreenVector2, headTexture.Bounds, Color.White, (float)world.VectorToRadian(direction), centerHead, 1f, SpriteEffects.None, 1f);
        }
        private void BaseDrawing(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(Point.Zero, backgroundTexture.Bounds.Size);
            int scaledWidth = backgroundTexture.Width;
            int scaledHeight = backgroundTexture.Height;

            for (int X = Viewport.X; X <= Viewport.Width; X += scaledWidth)
            {
                for (int Y = Viewport.Y; Y <= Viewport.Height; Y += scaledHeight)
                {
                    destinationRectangle.X = X - ((int)world.PlayerCharacter.DrawPosition.X % scaledWidth);
                    destinationRectangle.Y = Y - ((int)world.PlayerCharacter.DrawPosition.Y % scaledHeight);

                    spriteBatch.Draw(backgroundTexture, destinationRectangle, Color.White);
                }
            }

            spriteBatch.Draw(bigBushTexture, new Rectangle(Point.Zero - (world.PlayerCharacter.DrawPosition - centerScreen) - bigBushTexture.Bounds.Center, bigBushTexture.Bounds.Size), Color.White);
        }
    }
}
