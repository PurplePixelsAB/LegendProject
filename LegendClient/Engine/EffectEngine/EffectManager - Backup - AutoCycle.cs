using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Textures;
using Engine.EffectEngine.Light;
using Engine.EffectEngine.Particles;

namespace Engine.EffectEngine
{
    public class EffectManager : LegendaryComponent
    {
        internal EffectManager(LegendaryManager manager) : base(manager)
        {
        }

        public SpriteSheet EffectSheet { get { return particleEffectSheet; } set { particleEffectSheet = value; } }
        public SpriteSheet LoadEffectSheet(string SpriteSheetContentPath)
        {
            particleEffectSheet = new SpriteSheet(Manager.Game.Content.Load<SpriteSheetData>(SpriteSheetContentPath));
            return particleEffectSheet;
        }
        private SpriteSheet particleEffectSheet;

        public SpriteSheet LightSheet { get { return lightSheet; } set { lightSheet = value; } }
        public SpriteSheet LoadLightSheet(string SpriteSheetContentPath)
        {
            lightSheet = new SpriteSheet(Manager.Game.Content.Load<SpriteSheetData>(SpriteSheetContentPath));
            return lightSheet;
        }
        private SpriteSheet lightSheet;

        private RenderTarget2D lightSourceMap;
        private RenderTarget2D cloudShadowMap;

        internal Random Random { get; private set; }

        internal List<LightSource> LightSources { get; set; }
        internal List<ParticleEffect> Effects { get; set; }

        public float CycleProgress { get; set; }
        public void EnableMenualDayNightCycle(Color Day, Color Night)
        {
            CycleTime = Minutes * 60;
            CurrentCycleColor = Day;
            NextCycleColor = Night;
            CycleDrawColor = CurrentCycleColor;
            CycleStart = TimeSpan.Zero;
            UseDayNightCycle = true;
            UseAutoDayNightCycle = false;
        }

        public void EnableAutoDayNightCycle(int Minutes, Color Day, Color Night)
        {
            CycleTime = Minutes * 60;
            CurrentCycleColor = Day;
            NextCycleColor = Night;
            CycleDrawColor = CurrentCycleColor;
            CycleStart = TimeSpan.Zero;
            UseDayNightCycle = true;
            UseAutoDayNightCycle = true;
        }
        public void DisableAutoDayNightCycle(Color Gamma)
        {
            UseDayNightCycle = false;
            UseAutoDayNightCycle = false;
            CycleDrawColor = Gamma;
        }
        private bool UseDayNightCycle { get; set; }
        private bool UseAutoDayNightCycle { get; set; }
        private int CycleTime { get; set; }
        private Color CurrentCycleColor { get; set; }
        private Color NextCycleColor { get; set; }
        private Color CycleDrawColor { get; set; }
        private TimeSpan CycleStart { get; set; }

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
            lightSourceMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, SurfaceFormat.Color, pp.MultiSampleType, pp.MultiSampleQuality);
            cloudShadowMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, SurfaceFormat.Color, pp.MultiSampleType, pp.MultiSampleQuality);

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
            SpriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Deferred, SaveStateMode.None);
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
            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            GraphicsDevice.RenderState.SourceBlend = Blend.Zero;
            GraphicsDevice.RenderState.DestinationBlend = Blend.SourceColor;
            GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //SpriteBatch.Begin();

            //SpriteBatch.Draw(cloudShadowMap.GetTexture(), Vector2.Zero, Color.White);
            SpriteBatch.Draw(lightSourceMap.GetTexture(), Vector2.Zero, Color.White);

            SpriteBatch.End();
        }

        private void UpdateCycle(GameTime gameTime)
        {
            if (CycleStart == TimeSpan.Zero) //NewCycle
                CycleStart = gameTime.TotalGameTime;

            double ElapsedMilliseconds = gameTime.TotalGameTime.TotalMilliseconds - CycleStart.TotalMilliseconds;
            float ElapsedProgress = (float)(ElapsedMilliseconds / (CycleTime * 1000));
            float ColorProgress = Math.Min(1.0f, ElapsedProgress * 2.0f);

            byte r = (byte)(CurrentCycleColor.R - ((CurrentCycleColor.R - NextCycleColor.R) * ColorProgress));
            byte g = (byte)(CurrentCycleColor.G - ((CurrentCycleColor.G - NextCycleColor.G) * ColorProgress));
            byte b = (byte)(CurrentCycleColor.B - ((CurrentCycleColor.B - NextCycleColor.B) * ColorProgress));
            byte a = 255;
            CycleDrawColor = new Color(r, g, b, a);

            if (ElapsedProgress >= 1.0f)
                SwapCycle(gameTime);
        }
        private void SwapCycle(GameTime gameTime)
        {
            Color temp = CurrentCycleColor;
            CurrentCycleColor = NextCycleColor;
            NextCycleColor = temp;
            CycleStart = gameTime.TotalGameTime;
        }

        internal void CreateLightMap(GameTime gameTime)
        {
            if (lightSheet == null)
                return;

            GraphicsDevice.SetRenderTarget(0, lightSourceMap);
            GraphicsDevice.Clear(CycleDrawColor);

            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            GraphicsDevice.RenderState.DestinationBlend = Blend.One;
            GraphicsDevice.RenderState.SourceBlend = Blend.DestinationAlpha;
            GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = true;

            foreach (LightSource light in LightSources)
            {
                Rectangle sourceRectangle = lightSheet.GetSourceRectangle(light.Sprite); //SourceRectangle(light.Sprite);
                Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
                SpriteBatch.Draw(lightSheet.Texture, light.Position, sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
            }

            if (Manager.MapManager.Map != null)
                if (Manager.MapManager.Map.LightSource != null)
                {
                    {
                        foreach (LightSource light in Manager.MapManager.Map.LightSource)
                        {
                            Rectangle sourceRectangle = lightSheet.GetSourceRectangle(light.Sprite); //SourceRectangle(light.Sprite);
                            Vector2 center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                            float scale = light.Range / ((float)sourceRectangle.Width / 2.0f);
                            SpriteBatch.Draw(lightSheet.Texture, Manager.MapManager.MapToScreen(light.Position), sourceRectangle, light.Color, 0, center, scale, SpriteEffects.None, 1);
                        }
                    }
                }

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(0, null);
            GraphicsDevice.Clear(Color.White);
        }
    }
}
