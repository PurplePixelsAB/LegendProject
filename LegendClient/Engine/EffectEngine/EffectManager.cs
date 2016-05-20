using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Textures;
using Engine.EffectEngine.Light;
using Engine.EffectEngine.Particles;

namespace Engine.EffectEngine
{
    public class EffectManager //: LegendaryComponent
    {
        internal EffectManager()//(LegendaryManager manager) : base(manager)
        {
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
        internal List<ParticleEffect> Effects { get; set; }

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

        protected internal override void Initialize()
        {
            this.Random = new Random();
            LightSources = new List<LightSource>();
            Effects = new List<ParticleEffect>();

            base.Initialize();
        }

        protected internal override void LoadContent()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            lightSourceMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.None); // pp.MultiSampleType, pp.MultiSampleQuality);
            //cloudShadowMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, SurfaceFormat.Color, pp.MultiSampleType, pp.MultiSampleQuality);

            //particleEffectSheet = Manager.Game.Content.Load<SpriteSheet>("Textures\\Effect\\ParticleEffectSheet");
            //lightSheet = Manager.Game.Content.Load<SpriteSheet>("Textures\\Effect\\Light\\LightSheet");
        }
        protected internal override void UnloadContent()
        {
            particleEffectSheet = null;
            lightSheet = null;
            lightSourceMap.Dispose();

            LightSources.Clear();
            LightSources = null;
            Effects.Clear();
            Effects = null;
        }

        protected internal override void Update(GameTime gameTime)
        {
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                Effects[i].Update(gameTime);
                if (Effects[i].ParticleList.Count == 0)
                    Effects[i].Kill();
            }

            if (UseDayNightCycle)
                UpdateCycle(gameTime);
        }

        protected internal override void Draw(GameTime gameTime)
        {
            if (lightSheet != null)
                DrawLightMap(gameTime);

            if (particleEffectSheet != null)
                DrawEffects(gameTime);
        }
        private void DrawEffects(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //SpriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Deferred, SaveStateMode.None);
            for (int y = 0; y < Effects.Count; y++)
            {
                ParticleEffect effect = Effects[y];
                for (int i = 0; i < effect.ParticleList.Count; i++)
                {
                    Particle particle = effect.ParticleList[i];
                    Rectangle sourceRectangle = particleEffectSheet.GetSourceRectangle(effect.Sprite); //.SourceRectangle(effect.Sprite);
                    Vector2 origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                    SpriteBatch.Draw(particleEffectSheet.Texture, particle.Position, sourceRectangle, particle.ModColor, i, origin, particle.Scaling, SpriteEffects.None, 1);
                }
            }
            SpriteBatch.End();

        }
        private void DrawLightMap(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //GraphicsDevice.RenderState.SourceBlend = Blend.Zero;
            //GraphicsDevice.RenderState.DestinationBlend = Blend.SourceColor;
            //GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //SpriteBatch.Begin();

            //SpriteBatch.Draw(cloudShadowMap.GetTexture(), Vector2.Zero, Color.White);
            SpriteBatch.Draw(lightSourceMap, Vector2.Zero, Color.White);

            SpriteBatch.End();
        }

        private Color CycleDrawColor = Color.White;
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

            float ColorProgress = Math.Min(1.0f, CurrentStateProgress * 2.0f);

            byte r = (byte)(CurrentCycleColor.R - ((CurrentCycleColor.R - NextCycleColor.R) * ColorProgress));
            byte g = (byte)(CurrentCycleColor.G - ((CurrentCycleColor.G - NextCycleColor.G) * ColorProgress));
            byte b = (byte)(CurrentCycleColor.B - ((CurrentCycleColor.B - NextCycleColor.B) * ColorProgress));
            byte a = 255;
            CycleDrawColor = new Color(r, g, b, a);
        }

        internal void CreateLightMap(GameTime gameTime)
        {
            if (lightSheet == null)
                return;

            GraphicsDevice.SetRenderTarget(lightSourceMap);
            GraphicsDevice.Clear(CycleDrawColor);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //GraphicsDevice.RenderState.DestinationBlend = Blend.One;
            //GraphicsDevice.RenderState.SourceBlend = Blend.DestinationAlpha;
            //GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = true;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            

            foreach (LightSource light in LightSources)
            {
                Rectangle sourceRectangle = lightSheet.GetSourceRectangle(light.Sprite); //SourceRectangle(light.Sprite);
                Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
                SpriteBatch.Draw(lightSheet.Texture, light.Position, sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
            }

            if (Manager.MapManager.Map != null)
                if (Manager.MapManager.Map.LightSources != null)
                {
                    {
                        foreach (LightSource light in Manager.MapManager.Map.LightSources)
                        {
                            Rectangle sourceRectangle = lightSheet.GetSourceRectangle(light.Sprite); //SourceRectangle(light.Sprite);
                            Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                            float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
                            SpriteBatch.Draw(lightSheet.Texture, Manager.MapManager.MapToScreen(light.Position), sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
                        }
                    }
                }

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);
        }
    }
}
