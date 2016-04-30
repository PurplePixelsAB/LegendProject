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

namespace WindowsClient
{
    public class GameplayScreen : Screen
    {
        //public static TimeSpan TickSpeed = new TimeSpan(0, 0, 0, 0, 100);

        //private long lastTick;
        //private long nextTick;
        public bool IsConnected { get { return this.network.Connected; } }

        private SpriteBatch spriteBatch;
        private Texture2D bodyTexture;
        private Texture2D headTexture;
        private Texture2D leatherHoodTexture;
        private Texture2D leatherArmorTexture;
        private Texture2D bowTexture;
        private Texture2D backgroundTexture;

        //ClientCharacter character;
        //Rectangle headPosition, bodyPosition;
        Random rnd;
        //Vector2 rndHead;
        //Vector2 rndHeadTarget;
        //Vector2 rndHeadLast;
        //Vector2 headPosition;
        //private TimeSpan lastUpdateTime;
        //private int updateTime = 10;

        private NetworkEngine network;
        private ClientWorldState world;
        private WorldPump worldPump;
        private Texture2D selectionTexture;
        private SpriteFont damageSpriteFont;

        //ActionKeyMapping[] moveMappings;
        //ActionKeyMapping[] generalMappings;
        Rectangle Viewport;
        Vector2 CenterScreen;

        public GameplayScreen()
        {
            Viewport = new Rectangle(0, 0, 1920, 1080);
            CenterScreen = Viewport.Center.ToVector2();

            rnd = new Random();
            world = new ClientWorldState();
            network = new NetworkEngine();
            network.WorldState = world;
            worldPump = new WorldPump();
            worldPump.State = world;

            inventoryScreen = new InventoryScreen();
            inventoryScreen.ItemUsed += InventoryScreen_ItemUsed;
        }

        private void InventoryScreen_ItemUsed(object sender, ItemUsedEventArgs e)
        {
            if (e.ItemUsed.Category == LegendWorld.Data.ItemCategory.Consumable)
            {
                ConsumableItem consumable = (ConsumableItem)e.ItemUsed;
                consumable.Use(world.PlayerCharacter);
                network.UseItem(consumable);
            }
        }

        internal void SelectCharacter(ushort charId)
        {
            world.PlayerCharacter = new ClientCharacter();
            world.PlayerCharacter.Id = charId;
            world.PlayerCharacter.HealthChanged += PlayerCharacter_HealthChanged;
            world.AddCharacter(world.PlayerCharacter);
            network.SelectCharacter(charId);
        }

        public override void Initialize(ScreenManager screenManager)
        {
            base.Initialize(screenManager);
            inventoryScreen.Initialize(screenManager);
            network.Initialize();
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            network.LoadContent();
            inventoryScreen.LoadContent(graphicsDevice);   

            spriteBatch = new SpriteBatch(graphicsDevice);
            bodyTexture = Game.Content.Load<Texture2D>("Body");
            headTexture = Game.Content.Load<Texture2D>("Head");
            leatherHoodTexture = Game.Content.Load<Texture2D>("LeatherHood");
            leatherArmorTexture = Game.Content.Load<Texture2D>("LetherArmor");
            bowTexture = Game.Content.Load<Texture2D>("BowSmall");
            backgroundTexture = Game.Content.Load<Texture2D>("GrassBackground");
            selectionTexture = Game.Content.Load<Texture2D>("Selection");
            damageSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            bigBushTexture = Game.Content.Load<Texture2D>("bigbush");
            hudbarTexture = Game.Content.Load<Texture2D>("HudBar");

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
            actionKeyMappingAbility4.Primary = Keys.Q;
            actionKeyMappingAbility4.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility4);
            ActionKeyMapping actionKeyMappingAbility5 = new ActionKeyMapping();
            actionKeyMappingAbility5.Id = 15;
            actionKeyMappingAbility5.Primary = Keys.E;
            actionKeyMappingAbility5.ActionTriggered += actionKeyMappingAbility_ActionTriggered;
            Input.Actions.Add(actionKeyMappingAbility5);

            ActionKeyMapping actionKeyMappingOpenBags = new ActionKeyMapping();
            actionKeyMappingOpenBags.Id = 4;
            actionKeyMappingOpenBags.Primary = Keys.B;
            actionKeyMappingOpenBags.ActionTriggered += ActionKeyMappingOpenBags_ActionTriggered;
            Input.Actions.Add(actionKeyMappingOpenBags);

            ActionKeyMapping actionKeyMappingToggleFlullscreen = new ActionKeyMapping();
            actionKeyMappingToggleFlullscreen.Id = 0;
            actionKeyMappingToggleFlullscreen.Primary = Keys.Enter;
            actionKeyMappingToggleFlullscreen.PrimaryMod = Keys.LeftControl;
            actionKeyMappingToggleFlullscreen.ActionTriggered += ActionKeyMappingToggleFlullscreen_ActionTriggered;
            Input.Actions.Add(actionKeyMappingToggleFlullscreen);
        }
        
        private void ActionKeyMappingOpenBags_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            inventoryScreen.BaseContainer = (ClientBagItem)world.GetItem(world.PlayerCharacter.InventoryBagId);
            inventoryScreen.Activate();
        }

        private void ActionKeyMappingToggleFlullscreen_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            this.Game.Window.IsBorderless = !this.Game.Window.IsBorderless;
            this.Game.Window.Position = Point.Zero;
        }

        private void PlayerCharacter_HealthChanged(object sender, Character.HealthChangedEventArgs e)
        {
            ClientCharacter clientCharacter = (ClientCharacter)sender;
            if (clientCharacter.Health < e.PreviousHelth) //Damage
            {
                this.AddDamageIndicator(clientCharacter, e.PreviousHelth - clientCharacter.Health);
            }
            else //Healing
            {
                this.AddHealIndicator(clientCharacter, e.PreviousHelth - clientCharacter.Health);
            }
        }

        private void AddHealIndicator(ClientCharacter clientCharacter, int v)
        {
            //throw new NotImplementedException();
        }

        private Queue<DamageTextEffect> DamageTextEffectList = new Queue<DamageTextEffect>();
        private Texture2D bigBushTexture;

        private void AddDamageIndicator(ClientCharacter clientCharacter, int damageAmount)
        {
            var dmgEfx = new DamageTextEffect();
            dmgEfx.Character = clientCharacter;
            dmgEfx.Text = string.Format("{0}", damageAmount);
            dmgEfx.Duration = 2000;
            DamageTextEffectList.Enqueue(dmgEfx);
        }

        private void actionKeyMappingAbility_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            var abilityIndex = e.Action.Id - 10;
            if (world.PlayerCharacter.Abilities.Count > abilityIndex)
            {
                var abilityId = world.PlayerCharacter.Abilities[abilityIndex];

                if (world.PerformAbility(abilityId, world.PlayerCharacter))
                {
                    network.PerformAbility(world.PlayerCharacter, abilityId);
                }
            }
        }

        private void ActionButtonMappingAimTo_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            Point worldPosition = ScreenToWorld(e.MouseState.Position);
            if (world.PlayerCharacter.AimToPosition != worldPosition)
            {
                world.PlayerCharacter.SetAimToPosition(worldPosition);
                network.AimTo(worldPosition);
            }
        }

        private long lastMovementClickTicks;
        private TimeSpan doubleClickSpeed = new TimeSpan(0, 0, 0, 0, 200);
        private Texture2D hudbarTexture;

        private void ActionButtonMappingMoveTo_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            Point worldPosition = ScreenToWorld(e.MouseState.Position);

            if (world.PlayerCharacter.MovingToPosition != worldPosition)
            {
                world.PlayerCharacter.SetMoveToPosition(worldPosition);
                network.MoveTo(worldPosition);
            }

            if (lastMovementClickTicks + doubleClickSpeed.Ticks >= e.GameTime.TotalGameTime.Ticks)
            {
                if (world.PlayerCharacter.AimToPosition != worldPosition)
                {
                    world.PlayerCharacter.SetAimToPosition(worldPosition);
                    network.AimTo(worldPosition);
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
            worldPump.Update(gameTime);
            world.ClientUpdate(gameTime);
            network.Update();
            movementBodyBobEffect.Update(gameTime);
        }

        public void UpdateRandomBodyMovement(GameTime gameTime)
        {
            //ToDo: SAVE CODE - Random Body movement instead.
            //if ((lastUpdateTime.TotalSeconds + updateTime) <= gameTime.TotalGameTime.TotalSeconds)
            //{
            //    rndHeadLast = rndHeadTarget;
            //    rndHeadTarget = new Vector2(rnd.Next(-2, 2), rnd.Next(-1, 3));
            //    lastUpdateTime = gameTime.TotalGameTime;
            //    updateTime = rnd.Next(1, 3);
            //}
            //float lerp = (float)((gameTime.TotalGameTime.TotalSeconds - lastUpdateTime.TotalSeconds) / updateTime);
            //rndHead.X = (float)Math.Round(MathHelper.Lerp(rndHeadLast.X, rndHeadTarget.X, lerp));
            //rndHead.Y = (float)Math.Round(MathHelper.Lerp(rndHeadLast.Y, rndHeadTarget.Y, lerp));
        }

        //private float Vector2ToRadian(Vector2 direction)
        //{
        //    return (float)Math.Atan2(direction.X, -direction.Y) + MathHelper.Pi;
        //}

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            this.BaseDrawing(spriteBatch);
            this.DrawCharacters(spriteBatch);
            this.DrawDamageEffect(spriteBatch, gameTime);
            this.DrawHud(spriteBatch);
            spriteBatch.End();
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            Rectangle sourceSize = hudbarTexture.Bounds;

            var healthDrawPosition = new Vector2(CenterScreen.X - sourceSize.Center.X, CenterScreen.Y + (CenterScreen.Y * .7f));
            spriteBatch.Draw(hudbarTexture, healthDrawPosition, Color.White);

            int healthBasedWidth = (int)(sourceSize.Width * ((float)world.PlayerCharacter.Health / (float)world.PlayerCharacter.MaxHealth));
            Rectangle healthSize = sourceSize;
            healthSize.Width = healthBasedWidth;
            spriteBatch.Draw(hudbarTexture, healthDrawPosition, healthSize, Color.DarkGreen);

            var energyDrawPosition = healthDrawPosition + new Vector2(0f, sourceSize.Size.Y);
            spriteBatch.Draw(hudbarTexture, energyDrawPosition, Color.White);

            int energyBasedWidth = (int)(sourceSize.Width * ((float)world.PlayerCharacter.Energy / (float)world.PlayerCharacter.MaxEnergy));
            Rectangle energySize = sourceSize;
            energySize.Width = energyBasedWidth;
            spriteBatch.Draw(hudbarTexture, energyDrawPosition, energySize, Color.Orange);

            //foreach (int id in world.Characters)
            //{
            //    ClientCharacter charToDraw = (ClientCharacter)world.GetCharacter(id);
            //    Rectangle sourceSize = hudbarTexture.Bounds;
            //    int healthBasedWidth = (int)(sourceSize.Width * ((float)charToDraw.Health / (float)charToDraw.MaxHealth));
            //    sourceSize.Width = healthBasedWidth;
            //    spriteBatch.Draw(hudbarTexture, charToDraw.DrawPosition, Color.White);
            //    spriteBatch.Draw(hudbarTexture, charToDraw.DrawPosition, sourceSize, Color.DarkGreen);
            //}

            Vector2 centerSelection = selectionTexture.Bounds.Center.ToVector2();
            //Point screenCenter = new Point(960, 540);
            Vector2 drawAimLocation = CenterScreen - (world.PlayerCharacter.Position - world.PlayerCharacter.AimToPosition).ToVector2(); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);
            spriteBatch.Draw(selectionTexture, drawAimLocation, selectionTexture.Bounds, Color.Blue, 0f, centerSelection, 1f, SpriteEffects.None, 1f);

            if (world.PlayerCharacter.Position != world.PlayerCharacter.MovingToPosition)
            {
                Vector2 drawMoveToLocation = CenterScreen - (world.PlayerCharacter.Position - world.PlayerCharacter.MovingToPosition).ToVector2(); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);

                spriteBatch.Draw(selectionTexture, drawMoveToLocation, selectionTexture.Bounds, Color.Cyan, 0f, centerSelection, 1f, SpriteEffects.None, 1f);
            }

            //DrawWindow
            //if (inventory != null)
            //{
            //    inventory.Draw
            //    spriteBatch.Draw(inventory.BackgroundTexture, inventory.Location, inventory.BackgroundTexture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            //}


            spriteBatch.Draw(selectionTexture, Mouse.GetState().Position.ToVector2(), selectionTexture.Bounds, Color.Black, 0f, centerSelection, 1f, SpriteEffects.None, 1f);
        }
        private void DrawDamageEffect(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var queue = DamageTextEffectList;
            DamageTextEffectList = new Queue<DamageTextEffect>();
            while (queue.Count > 0)
            {
                var effect = queue.Dequeue();
                //Vector2 centerVector2 = new Vector2(960f, 540f);
                Vector2 charTakingDmgLocation = effect.Character.Position.ToVector2();
                Vector2 effectDrawLocation = CenterScreen - (world.PlayerCharacter.Position.ToVector2() - charTakingDmgLocation); //new Vector2(world.PlayerCharacter.Position.X - charTakingDmgLocation.X, world.PlayerCharacter.Position.Y - charTakingDmgLocation.Y);
                spriteBatch.DrawString(damageSpriteFont, effect.Text, effectDrawLocation, Color.Red);
                effect.Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;

                if (effect.Duration > 0D)
                    DamageTextEffectList.Enqueue(effect);
            }
        }


        public class MovementBodyBobEffect
        {
            //float lerp = 0f;
            float lerpSpeed = .01f;

            public Vector2 PositioinBob { get; set; }
            public Vector2 BobValue { get; set; }

            public MovementBodyBobEffect()
            {
                BobValue = new Vector2(0f, 2f);
                PositioinBob = new Vector2(0f, 0f);
            }
                

            public void Update(GameTime gameTime)
            {
                double msElapsed = gameTime.TotalGameTime.Milliseconds;
                var offset = (float)Math.Sin(msElapsed * lerpSpeed);

                this.PositioinBob = Vector2.Lerp(this.BobValue, this.BobValue*-1f, offset);
            }
        }
        private MovementBodyBobEffect movementBodyBobEffect = new MovementBodyBobEffect();
        private Texture2D bagTexture;
        private InventoryScreen inventoryScreen;

        private void DrawCharacters(SpriteBatch spriteBatch)
        {
            Vector2 centerHead = headTexture.Bounds.Center.ToVector2();
            Vector2 centerBody = bodyTexture.Bounds.Center.ToVector2();
            centerBody.Y = 0f;

            foreach (ushort id in world.Characters)
            {
                if (id == world.PlayerCharacter.Id)
                    continue;

                ClientCharacter charToDraw = (ClientCharacter)world.GetCharacter(id);
                Vector2 charToDrawDirection = charToDraw.Position.ToVector2() - charToDraw.AimToPosition.ToVector2();
                Vector2 bodyMovingBobPosition = Vector2.Zero;
                if (charToDraw.IsMoving)
                    bodyMovingBobPosition = movementBodyBobEffect.PositioinBob;

                Vector2 bodyRotationPosition = charToDrawDirection;
                bodyRotationPosition.Normalize();
                bodyRotationPosition.X *= 2f;
                if (bodyRotationPosition.Y < 0f)
                    bodyRotationPosition.Y *= 10f;

                spriteBatch.Draw(bodyTexture, charToDraw.DrawPosition + bodyMovingBobPosition + bodyRotationPosition, null, Color.White, 0f, centerBody, 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(headTexture, charToDraw.DrawPosition, null, Color.White, (float)world.VectorToRadian(charToDrawDirection), centerHead, 1f, SpriteEffects.None, 1f);
            }

            Vector2 start = world.PlayerCharacter.Position.ToVector2();
            Vector2 end = world.PlayerCharacter.AimToPosition.ToVector2();
            //float distance = Vector2.Distance(start, end);
            Vector2 direction = end - start;

            Vector2 plrbodyMovingBobPosition = Vector2.Zero;
            if (world.PlayerCharacter.IsMoving)
                plrbodyMovingBobPosition = movementBodyBobEffect.PositioinBob;

            Vector2 plrbodyRotationPosition = direction;
            plrbodyRotationPosition.Normalize();
            plrbodyRotationPosition.X *= 2f;
            if (plrbodyRotationPosition.Y < 0f)
            plrbodyRotationPosition.Y *= 10f;

            spriteBatch.Draw(bodyTexture, CenterScreen + plrbodyMovingBobPosition + plrbodyRotationPosition, null, Color.White, 0f, centerBody, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(headTexture, CenterScreen, headTexture.Bounds, Color.White, (float)world.VectorToRadian(direction), centerHead, 1f, SpriteEffects.None, 1f);
        }
        private void BaseDrawing(SpriteBatch spriteBatch)
        {
            Vector2 destinationVector = new Vector2();
            int scaledWidth = backgroundTexture.Width;
            int scaledHeight = backgroundTexture.Height;

            for (int X = Viewport.X; X <= Viewport.Width; X += scaledWidth)
            {
                for (int Y = Viewport.Y; Y <= Viewport.Height; Y += scaledHeight)
                {
                    destinationVector.X = X - ((int)world.PlayerCharacter.Position.X % scaledWidth);
                    destinationVector.Y = Y - ((int)world.PlayerCharacter.Position.Y % scaledHeight);

                    spriteBatch.Draw(backgroundTexture, destinationVector, Color.White);
                }
            }

            spriteBatch.Draw(bigBushTexture, Vector2.Zero - (world.PlayerCharacter.Position.ToVector2() - CenterScreen) - bigBushTexture.Bounds.Center.ToVector2(), Color.White);
        }
    }
}
