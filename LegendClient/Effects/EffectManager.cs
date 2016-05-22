using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.Effects
{
    public class EffectManager //: LegendaryComponent
    {
        internal EffectManager()//(LegendaryManager manager) : base(manager)
        {
            this.Random = new Random();
            LightSources = new List<LightSource>();
            Effects = new List<ParticleEffect>();
        }

        internal void AddEffect(ParticleEffect particleEffect)
        {
            this.newEffects.Enqueue(particleEffect);
        }

        internal void RemoveEffect(ParticleEffect particleEffect)
        {
            if (!this.Effects.Contains(particleEffect))
                return;

            this.Effects.Remove(particleEffect);
        }

        //public SpriteSheet EffectSheet { get { return particleEffectSheet; } set { particleEffectSheet = value; } }
        //public SpriteSheet LoadEffectSheet(string SpriteSheetContentPath)
        //{
        //    particleEffectSheet = new SpriteSheet(Manager.Game.Content.Load<SpriteSheetData>(SpriteSheetContentPath));
        //    return particleEffectSheet;
        //}
        //private SpriteSheet particleEffectSheet;

        //public SpriteSheet LightSheet { get { return lightSheet; } set { lightSheet = value; } }
        //public SpriteSheet LoadLightSheet(string SpriteSheetContentPath)
        //{
        //    lightSheet = new SpriteSheet(Manager.Game.Content.Load<SpriteSheetData>(SpriteSheetContentPath));
        //    return lightSheet;
        //}
        //private SpriteSheet lightSheet;

        private RenderTarget2D lightSourceMap;

        internal Random Random { get; private set; }

        internal List<LightSource> LightSources { get; set; }
        private List<ParticleEffect> Effects { get; set; }
        private Queue<ParticleEffect> newEffects = new Queue<ParticleEffect>(30);

        public bool UseDayNightCycle { get; set; }
        public enum CycleState
        {
            Day,
            Night
        }
        public CycleState CurrentState { get; set; }
        public Color DayColor { get; set; }
        public Color NightColor { get; set; }
        public float CurrentStateProgress { get; set; }

        private Color CycleDrawColor = Color.White;

        private Dictionary<string, Texture2D> effectTextures;

        //protected internal void Initialize()
        //{
        //}

        protected internal void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            lightSourceMap = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.None); // pp.MultiSampleType, pp.MultiSampleQuality);
            //cloudShadowMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, SurfaceFormat.Color, pp.MultiSampleType, pp.MultiSampleQuality);

            //particleEffectSheet = Manager.Game.Content.Load<SpriteSheet>("Textures\\Effect\\ParticleEffectSheet");
            //lightSheet = Manager.Game.Content.Load<SpriteSheet>("Textures\\Effect\\Light\\LightSheet");
            effectTextures = new Dictionary<string, Texture2D>();
            effectTextures.Add("SoftRound", content.Load<Texture2D>("Lights/SoftRoundLight"));
            effectTextures.Add("StrongRound", content.Load<Texture2D>("Lights/StrongRoundLight"));
            effectTextures.Add("Star", content.Load<Texture2D>("Lights/StarLight"));
            effectTextures.Add("Cross", content.Load<Texture2D>("Lights/CrossLight"));
            effectTextures.Add("Swing", content.Load<Texture2D>("Swing")); 
            effectTextures.Add("BloodSplatter01", content.Load<Texture2D>("BloodSplatter01")); 

        }
        protected internal void UnloadContent()
        {
            //particleEffectSheet = null;
            //lightSheet = null;
            lightSourceMap.Dispose();

            LightSources.Clear();
            LightSources = null;
            Effects.Clear();
            Effects = null;
        }

        protected internal void Update(GameTime gameTime)
        {
            while(newEffects.Count > 0)
            {
                var effect = newEffects.Dequeue();
                effect.Create(this, gameTime);
                this.Effects.Add(effect);
            }

            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                Effects[i].Update(gameTime);
                if (Effects[i].ParticleList.Count == 0)
                    Effects[i].Kill();
            }

            if (UseDayNightCycle)
                UpdateCycle(gameTime);
        }

        protected internal void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //if (lightSheet != null)
                DrawLightMap(spriteBatch, gameTime);

            //if (particleEffectSheet != null)
                DrawEffects(spriteBatch, gameTime);
        }
        private void DrawEffects(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //SpriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Deferred, SaveStateMode.None);
            for (int y = 0; y < Effects.Count; y++)
            {
                ParticleEffect effect = Effects[y];
                for (int i = 0; i < effect.ParticleList.Count; i++)
                {
                    Particle particle = effect.ParticleList[i];
                    Texture2D textureToDraw = effectTextures[effect.Sprite];
                    Rectangle sourceRectangle = textureToDraw.Bounds; //particleEffectSheet.GetSourceRectangle(effect.Sprite);
                    Vector2 origin = sourceRectangle.Center.ToVector2(); //new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                    spriteBatch.Draw(textureToDraw, particle.Position + new Vector2(20f, 20f), sourceRectangle, particle.Color, particle.Rotation, origin, particle.Scale, SpriteEffects.None, 1);
                }
            }
            spriteBatch.End();

        }
        BlendState Multiply = new BlendState()
        {
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.SourceAlpha,
            AlphaBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.SourceColor,
            ColorBlendFunction = BlendFunction.Add
        };
        private float cycleInterval = 120000;

        private void DrawLightMap(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, Multiply); // BlendState.AlphaBlend);
            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //GraphicsDevice.RenderState.SourceBlend = Blend.Zero;
            //GraphicsDevice.RenderState.DestinationBlend = Blend.SourceColor;
            //GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //SpriteBatch.Begin();

            //SpriteBatch.Draw(cloudShadowMap.GetTexture(), Vector2.Zero, Color.White);
            spriteBatch.Draw(lightSourceMap, Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        private void UpdateCycle(GameTime gameTime)
        {
            Color CurrentCycleColor = Color.TransparentBlack;
            Color NextCycleColor = Color.TransparentBlack;
            if (CurrentState == CycleState.Day)
            {
                CurrentCycleColor = DayColor;
                NextCycleColor = NightColor;
            }
            else
            {
                CurrentCycleColor = NightColor;
                NextCycleColor = DayColor;
            }

            CurrentStateProgress += (float)gameTime.ElapsedGameTime.TotalMilliseconds / cycleInterval;
                float ColorProgress = Math.Min(1.0f, CurrentStateProgress * 2.0f);

            byte r = (byte)(CurrentCycleColor.R - ((CurrentCycleColor.R - NextCycleColor.R) * ColorProgress));
            byte g = (byte)(CurrentCycleColor.G - ((CurrentCycleColor.G - NextCycleColor.G) * ColorProgress));
            byte b = (byte)(CurrentCycleColor.B - ((CurrentCycleColor.B - NextCycleColor.B) * ColorProgress));
            byte a = 255;
            CycleDrawColor = new Color(r, g, b, a);

            if (CurrentStateProgress > 1f)
            {
                CurrentStateProgress = 0f;
                CurrentState = CurrentState == CycleState.Day ? CycleState.Night : CycleState.Day;
            }
        }

        internal void CreateLightMap(GraphicsDevice graphicsDevice, SpriteBatch SpriteBatch, GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(lightSourceMap);
            graphicsDevice.Clear(CycleDrawColor);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.DepthRead);
            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //GraphicsDevice.RenderState.DestinationBlend = Blend.One;
            //GraphicsDevice.RenderState.SourceBlend = Blend.DestinationAlpha;
            //GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = true;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.AlphaBlend;


            foreach (LightSource light in LightSources)
            {
                Texture2D textureToDraw = effectTextures[light.Sprite];
                Rectangle sourceRectangle = textureToDraw.Bounds; //lightSheet.GetSourceRectangle(light.Sprite);
                Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
                SpriteBatch.Draw(textureToDraw, light.Position, sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
            }

            //if (Manager.MapManager.Map != null)
            //    if (Manager.MapManager.Map.LightSources != null)
            //    {
            //        {
            //            foreach (LightSource light in Manager.MapManager.Map.LightSources)
            //            {
            //                Rectangle sourceRectangle = lightSheet.GetSourceRectangle(light.Sprite); //SourceRectangle(light.Sprite);
            //                Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
            //                float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
            //                SpriteBatch.Draw(lightSheet.Texture, Manager.MapManager.MapToScreen(light.Position), sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
            //            }
            //        }
            //    }

            SpriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Transparent);
        }
    }
}
