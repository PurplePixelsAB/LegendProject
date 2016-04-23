using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Textures;

namespace Engine.EffectEngine.LightEngine
{
    internal class LightManager
    {
        internal LightManager(LegendaryManager manager, EffectManager effect) 
        {
            if (manager == null)
                throw new NullReferenceException("LegendaryManager is null.");

            this.Manager = manager;
            this.EffectManager = effect;
        }
        private LegendaryManager Manager { get; set; }
        private EffectManager EffectManager { get; set; }
        private GraphicsDevice GraphicsDevice { get { return Manager.GraphicsDevice; } }
        private SpriteBatch SpriteBatch { get { return Manager.SpriteBatch; } }

        private SpriteSheet lightSheet;
        private RenderTarget2D lightMap;

        private LinkedList<DayCycle> Cycles;
        private LinkedListNode<DayCycle> CurrentCycle;

        //private DayCycle[] Cycles;
        private int CycleIndex = 0;        
        private Color CurrentColor;
        private int TotalDayCycleTime = 60;

        internal void Initialize()
        {
            //Cycles = new DayCycle[4];
        }

        internal void LoadContent()
        {
            Cycles = new LinkedList<DayCycle>();
            Cycles.AddFirst(Manager.Game.Content.Load<DayCycle>("Effects\\Light\\DayNightCycle\\Dawn"));
            Cycles.AddAfter(Cycles.First, Manager.Game.Content.Load<DayCycle>("Effects\\Light\\DayNightCycle\\Day"));
            Cycles.AddLast(Manager.Game.Content.Load<DayCycle>("Effects\\Light\\DayNightCycle\\Night"));
            Cycles.AddBefore(Cycles.Last, Manager.Game.Content.Load<DayCycle>("Effects\\Light\\DayNightCycle\\Dusk"));

            CurrentCycle = Cycles.First;
            CurrentColor = CurrentCycle.Value.Color;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            lightMap = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.None); // pp.MultiSampleType, pp.MultiSampleQuality);
            lightSheet = Manager.Game.Content.Load<SpriteSheet>("Textures\\Effect\\Light\\LightSheet");
        }
        internal void UnloadContent()
        {
            lightMap.Dispose();
            lightMap = null;
        }

        internal void Update(GameTime gameTime)
        {
            float currentCycleLifeTime = (float)(gameTime.TotalGameTime.TotalSeconds - CurrentCycle.Value.BirthTime.TotalSeconds);
            float currentCycleTime = TotalDayCycleTime * CurrentCycle.Value.CycleTime;
            float currentCycleProgress = currentCycleLifeTime / currentCycleTime;
            Color compareColor;
            if (currentCycleProgress >= 0.5f)
                compareColor = CurrentCycle.Next != null ? CurrentCycle.Next.Value.Color : Cycles.First.Value.Color;
            else
                compareColor = CurrentCycle.Previous != null ? CurrentCycle.Previous.Value.Color : Cycles.Last.Value.Color;

            
            byte r = (byte)(CurrentCycle.Value.Color.R - ((CurrentCycle.Value.Color.R-compareColor.R)*currentCycleProgress));
            byte g = (byte)(CurrentCycle.Value.Color.G - ((CurrentCycle.Value.Color.G-compareColor.G)*currentCycleProgress));
            byte b = (byte)(CurrentCycle.Value.Color.B - ((CurrentCycle.Value.Color.B - compareColor.B) * currentCycleProgress));
            byte a = 255;
            CurrentColor = new Color(r, g, b, a);


            Console.WriteLine(CurrentCycle.Value.Name + " " + currentCycleProgress + " " + CurrentColor.R + " " + CurrentColor.G + " " + CurrentColor.B + " " + CurrentColor.A);

            //Vector3 newColor = new Vector3();
            //CurrentCycle.Value.Color
            //Vector3 colorDiff = colorCurr - compareColor;
            //color.X = colorCurr.X + (colorDiff.X);
            //color.Y = colorCurr.Y + (colorDiff.Y * (currentCycleProgress + 0.5f));
            //color.Z = colorCurr.Z + (colorDiff.Z * (currentCycleProgress + 0.5f));
            //CurrentColor = new Color(color, 255);
            
            if (currentCycleLifeTime >= currentCycleTime)
            {
                CurrentCycle = CurrentCycle.Next != null ? CurrentCycle.Next : Cycles.First;
                CurrentCycle.Value.BirthTime = gameTime.TotalGameTime;
            }

            //Night     new Color(75, 75, 100);
            //Day       new Color(255, 255, 255); 
            //Dawn      new Color(255, 200, 100);
            //Dusk      new Color(150, 150, 200);
        }

        internal void Draw(GameTime gameTime)
        {
            //this.DrawLightMap();
        }

        internal void DrawLightMap()
        {
        }
    }
}
