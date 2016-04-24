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

namespace WindowsClient
{
    public class GameplayScreen : Screen
    {
        //public static TimeSpan TickSpeed = new TimeSpan(0, 0, 0, 0, 100);

        //private long lastTick;
        //private long nextTick;

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
        }

        public override void LoadContent(GraphicsDevice graphicsDevice)
        {
            network.LoadContent();

            spriteBatch = new SpriteBatch(graphicsDevice);
            bodyTexture = Game.Content.Load<Texture2D>("Body");
            headTexture = Game.Content.Load<Texture2D>("Head");
            leatherHoodTexture = Game.Content.Load<Texture2D>("LeatherHood");
            leatherArmorTexture = Game.Content.Load<Texture2D>("LetherArmor");
            bowTexture = Game.Content.Load<Texture2D>("BowSmall");
            backgroundTexture = Game.Content.Load<Texture2D>("GrassBackground");
            selectionTexture = Game.Content.Load<Texture2D>("Selection");
            damageSpriteFont = Game.Content.Load<SpriteFont>("Damage");
            bigBushSpriteFont = Game.Content.Load<Texture2D>("bigbush");
            hudbarTexture = Game.Content.Load<Texture2D>("HudBar");

            //generalMappings = Game.Content.Load<ActionKeyMapping[]>("DefaultKeys\\General");
            //for (int i = 0; i <= generalMappings.GetUpperBound(0); i++)
            //{
            //    generalMappings[i].ActionTriggered += new ActionTriggeredEventHandler(GeneralKeys_ActionTriggered);
            //}
            //Input.Actions.AddRange(generalMappings);

            world.PlayerCharacter = new ClientCharacter();
            world.PlayerCharacter.Id = rnd.Next(1, Int16.MaxValue);
            world.PlayerCharacter.HealthChanged += PlayerCharacter_HealthChanged;
            world.AddCharacter(world.PlayerCharacter);

            DateTime timeOut = DateTime.Now.AddSeconds(20);
            while (!network.Connected && DateTime.Now < timeOut)
            {
                Thread.Sleep(500);
            }
            if (!network.Connected)
            {
                this.Game.Exit();
                return;
            }
            network.SelectCharacter(world.PlayerCharacter.Id);

            ActionButtonMapping actionButtonMappingMoveTo = new ActionButtonMapping();
            actionButtonMappingMoveTo.Action = 1;
            actionButtonMappingMoveTo.Primary = MouseButtons.Right;
            actionButtonMappingMoveTo.ActionTriggered += ActionButtonMappingMoveTo_ActionTriggered;
            Input.Actions.Add(actionButtonMappingMoveTo);

            ActionButtonMapping actionButtonMappingAimTo = new ActionButtonMapping();
            actionButtonMappingAimTo.Action = 2;
            actionButtonMappingAimTo.Primary = MouseButtons.Left;
            actionButtonMappingAimTo.ActionTriggered += ActionButtonMappingAimTo_ActionTriggered;
            Input.Actions.Add(actionButtonMappingAimTo);

            ActionKeyMapping actionKeyMappingSwing = new ActionKeyMapping();
            actionKeyMappingSwing.Action = 3;
            actionKeyMappingSwing.Primary = Keys.D1;
            actionKeyMappingSwing.ActionTriggered += ActionKeyMappingSwing_ActionTriggered;
            Input.Actions.Add(actionKeyMappingSwing);
            
            ActionKeyMapping actionKeyMappingToggleFlullscreen = new ActionKeyMapping();
            actionKeyMappingToggleFlullscreen.Action = 0;
            actionKeyMappingToggleFlullscreen.Primary = Keys.Enter;
            actionKeyMappingToggleFlullscreen.PrimaryMod = Keys.LeftControl;
            actionKeyMappingToggleFlullscreen.ActionTriggered += ActionKeyMappingToggleFlullscreen_ActionTriggered;
            Input.Actions.Add(actionKeyMappingToggleFlullscreen);
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
        private Texture2D bigBushSpriteFont;

        private void AddDamageIndicator(ClientCharacter clientCharacter, int damageAmount)
        {
            var dmgEfx = new DamageTextEffect();
            dmgEfx.Character = clientCharacter;
            dmgEfx.Text = string.Format("{0}", damageAmount);
            dmgEfx.Duration = 2000;
            DamageTextEffectList.Enqueue(dmgEfx);
        }

        private void ActionKeyMappingSwing_ActionTriggered(object sender, ActionTriggeredEventArgs e)
        {
            world.PerformSwing(world.PlayerCharacter);
            network.PerformSwing(world.PlayerCharacter);
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

            if (lastMovementClickTicks + doubleClickSpeed.Ticks <= e.GameTime.TotalGameTime.Ticks)
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

        private float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y) + MathHelper.Pi;
        }
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
            foreach (int id in world.Characters)
            {
                ClientCharacter charToDraw = (ClientCharacter)world.GetCharacter(id);
                Rectangle sourceSize = hudbarTexture.Bounds;
                int healthBasedWidth = (int)(sourceSize.Width * ((float)charToDraw.Health / (float)charToDraw.MaxHealth));
                sourceSize.Width = healthBasedWidth;
                spriteBatch.Draw(hudbarTexture, charToDraw.DrawPosition, Color.White);
                spriteBatch.Draw(hudbarTexture, charToDraw.DrawPosition, sourceSize, Color.DarkGreen);
            }

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

        private void DrawCharacters(SpriteBatch spriteBatch)
        {
            //Rectangle screen = new Rectangle(0, 0, 1920, 1080);
            //Rectangle charBox = new Rectangle(screen.Center.X - 12, screen.Center.Y - 25, 24, 50);
            //Vector2 centerVector2 = new Vector2(960f, 540f);

            //Vector2 centerBox = screen.Center.ToVector2();
            Vector2 centerHead = headTexture.Bounds.Center.ToVector2();
            Vector2 centerSelection = selectionTexture.Bounds.Center.ToVector2();
            //headPosition = centerBox - centerHead + rndHead;

            //Vector2 centerBody = bodyTexture.Bounds.Center.ToVector2();
            //Vector2 bodyPosition = new Vector2();
            //bodyPosition.X = centerBox.X - centerBody.X;
            //bodyPosition.Y = centerBox.Y + (centerBody.Y * .4f);

            foreach (int id in world.Characters)
            {
                if (id == world.PlayerCharacter.Id)
                    continue;

                ClientCharacter charToDraw = (ClientCharacter)world.GetCharacter(id);
                Vector2 charToDrawDirection = charToDraw.Position.ToVector2() - charToDraw.AimToPosition.ToVector2();
                //Vector2 charToDrawLocation = centerVector2 - new Vector2(world.PlayerCharacter.Position.X - charToDraw.ServerPosition.X, world.PlayerCharacter.Position.Y - charToDraw.ServerPosition.Y);
                spriteBatch.Draw(headTexture, charToDraw.DrawPosition, null, Color.White, Vector2ToRadian(charToDrawDirection), centerHead, 1f, SpriteEffects.None, 1f);
            }

            //#if DEBUG
            //            Vector2 debugDrawLocation = centerVector2 + new Vector2(world.PlayerCharacter.ServerPosition.X, world.PlayerCharacter.ServerPosition.Y) - new Vector2(world.PlayerCharacter.MapPoint.X, world.PlayerCharacter.MapPoint.Y);
            //            //spriteBatch.Draw(bodyTexture, bodyPosition, Color.Red);
            //            spriteBatch.Draw(headTexture, debugDrawLocation, Color.Red);
            //#endif

            Vector2 start = world.PlayerCharacter.Position.ToVector2();
            Vector2 end = world.PlayerCharacter.AimToPosition.ToVector2();
            //float distance = Vector2.Distance(start, end);
            Vector2 direction = end - start;
            //double drawAngle = Math.Atan2(direction.X, direction.Y);
            //float rotation = (float)(drawAngle + (Math.PI * 0.5D));
            //spriteBatch.Draw(bodyTexture, bodyPosition, Color.White);
            spriteBatch.Draw(headTexture, CenterScreen, headTexture.Bounds, Color.White, Vector2ToRadian(direction), centerHead, 1f, SpriteEffects.None, 1f);


            //Point screenCenter = new Point(960, 540);
            Vector2 drawAimLocation = CenterScreen - (world.PlayerCharacter.Position - world.PlayerCharacter.AimToPosition).ToVector2(); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);
            spriteBatch.Draw(selectionTexture, drawAimLocation, selectionTexture.Bounds, Color.Blue, 0f, centerSelection, 1f, SpriteEffects.None, 1f);

            if (world.PlayerCharacter.Position != world.PlayerCharacter.MovingToPosition)
            {
                Vector2 drawMoveToLocation = CenterScreen - (world.PlayerCharacter.Position - world.PlayerCharacter.MovingToPosition).ToVector2(); //new Vector2(world.PlayerCharacter.Position.X - world.PlayerCharacter.AimToPosition.X, world.PlayerCharacter.Position.Y - world.PlayerCharacter.AimToPosition.Y);

                spriteBatch.Draw(selectionTexture, drawMoveToLocation, selectionTexture.Bounds, Color.Cyan, 0f, centerSelection, 1f, SpriteEffects.None, 1f);
            }
            spriteBatch.Draw(selectionTexture, Mouse.GetState().Position.ToVector2(), selectionTexture.Bounds, Color.Black, 0f, centerSelection, 1f, SpriteEffects.None, 1f);
        }
        private void BaseDrawing(SpriteBatch spriteBatch)
        {
            //Vector2 centerVector2 = new Vector2(960f, 540f);

            //Rectangle screen = new Rectangle(0, 0, 1920, 1080);
            //Vector2 drawLocation = centerVector2 - world.PlayerCharacter.DrawPosition; //.Position.ToVector2(); //new Vector2(world.PlayerCharacter.Position.X, world.PlayerCharacter.Position.Y);
            //spriteBatch.Draw(backgroundTexture, world.PlayerCharacter.DrawPosition, Color.White);

            //if (Map.Base == null)
            //    throw new ArgumentNullException("Invalid or no Base Sprite selected.");

            //Vector2 destinationVector = Vector2.Zero;

            //int scaledWidth = (int)(Map.Base.Source.Width);
            //int scaledHeight = (int)(Map.Base.Source.Height);

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

            spriteBatch.Draw(bigBushSpriteFont, Vector2.Zero - (world.PlayerCharacter.Position.ToVector2() - CenterScreen) - bigBushSpriteFont.Bounds.Center.ToVector2(), Color.White);

        }

    }
}
