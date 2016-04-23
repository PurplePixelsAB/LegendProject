using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Engine.Textures;
using Engine.ScreenEngine;

namespace Engine.MapEngine
{
    public class MapManager : LegendaryComponent
    {
        internal MapManager(LegendaryManager manager)
            : base(manager)
        {
            DrawBase = true;
            DrawItems = true;
            Scale = 1f;
        }

        public Rectangle Viewport { get; set; }
        public Vector2 Camera { get; set; }
        public IMap Map { get; set; }

        public bool DrawBase { get; set; }
        //public bool ShowBaseLines { get; set; }

        public bool DrawItems { get; set; }
        //public bool ShowDecorLines { get; set; }

        //public bool ShowNpc { get; set; }

        //public bool ShowBlock { get; set; }

        public Vector2 GridSize { get; set; }
        public float Scale { get; set; }
        
        public ContentManager Content;

        private OnePixelTexture whitePixel;

        private List<IMapDrawable> DrawList = new List<IMapDrawable>(100);

        protected internal override void Initialize()
        {
            Content = new ContentManager(Manager.Game.Services, "Content");
            whitePixel = new OnePixelTexture(Manager.GraphicsDevice);
            Viewport = Rectangle.Empty;
            Camera = Vector2.Zero;

            base.Initialize();
        }

        protected internal override void Update(GameTime gameTime)
        {
            DrawList.Clear();

            if ((!IsInitialized) || (this.Map == null) || (this.Viewport == Rectangle.Empty))
                return;

            float revScale = 2.0f - Scale;

            int scaledViewportWidth = (int)(Viewport.Width * revScale);
            int scaledViewportHeight = (int)(Viewport.Height * revScale);

            Rectangle viewRectangle = new Rectangle((int)Camera.X - (int)(scaledViewportWidth / 2), (int)Camera.Y - (int)(scaledViewportHeight / 2), scaledViewportWidth + (int)(scaledViewportWidth / 2), scaledViewportHeight + (int)(scaledViewportHeight / 2));

            for (int LayerIndex = 0; LayerIndex <= Map.LayerDrawing.GetUpperBound(0); LayerIndex++)
            {
                for (int ItemIndex = 0; ItemIndex <= Map.LayerDrawing[LayerIndex].GetUpperBound(0); ItemIndex++)
                {
                    IMapDrawable item = Map.LayerDrawing[LayerIndex][ItemIndex];
                    Rectangle sourceRectangle = item.Sprite.Source;
                    Rectangle decorRectangle = new Rectangle((int)item.Position.X, (int)item.Position.Y, (int)(sourceRectangle.Width * item.Scale * Scale), (int)(sourceRectangle.Height * item.Scale * Scale));

                    if (decorRectangle.Intersects(viewRectangle))
                        DrawList.Add(item);
                }
            }
        }

        protected internal override void Draw(GameTime gameTime)
        {
            if ((!IsInitialized) || (this.Map == null) || (this.Viewport == Rectangle.Empty))
                return;

            if (DrawBase)
                this.BaseDrawing();

            if (DrawItems)
                this.ItemDrawing();
        }

        private void BaseDrawing()
        {
            if (Map.Base == null)
                throw new ArgumentNullException("Invalid or no Base Sprite selected.");

            Vector2 destinationVector = Vector2.Zero;

            int scaledWidth = (int)(Map.Base.Source.Width * Scale);
            int scaledHeight = (int)(Map.Base.Source.Height * Scale);

            for (int X = Viewport.X; X < Viewport.Width + Camera.X; X += scaledWidth)
            {
                for (int Y = Viewport.Y; Y < Viewport.Height + Camera.Y; Y += scaledHeight)
                {
                    destinationVector.X = X - ((int)Camera.X % scaledWidth);
                    destinationVector.Y = Y - ((int)Camera.Y % scaledHeight);

                    SpriteBatch.Draw(Map.Base.Texture, destinationVector, Map.Base.Source, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
            }
        }

        private void ItemDrawing()
        {
            Vector2 destinationVector = Vector2.Zero;

            for (int i = 0; i < DrawList.Count; i++)
            {
                IMapDrawable item = DrawList[i];

                destinationVector.X = item.Position.X - Camera.X + Viewport.Center.X;
                destinationVector.Y = item.Position.Y - Camera.Y + Viewport.Center.Y;

                SpriteBatch.Draw(item.Sprite.Texture, destinationVector, item.Sprite.Source, Color.White, (MathHelper.Pi * 2) * item.Rotation, new Vector2(0, 0), item.Scale * Scale, SpriteEffects.None, item.Layer);
            }
        }

        public Vector2 MapToScreen(Vector2 Position)
        {
            return MapToScreen(Position.X, Position.Y);
        }
        public Vector2 MapToScreen(int X, int Y)
        {
            return MapToScreen((float)X, (float)Y);
        }
        public Vector2 MapToScreen(float X, float Y)
        {
            Vector2 screenPosition = new Vector2();
            screenPosition.X = X - Camera.X + Viewport.Center.X;
            screenPosition.Y = Y - Camera.Y + Viewport.Center.Y;
            return screenPosition;
        }

        public Vector2 ScreenToMap(Vector2 Position)
        {
            return ScreenToMap(Position.X, Position.Y);
        }
        public Vector2 ScreenToMap(int X, int Y)
        {
            return ScreenToMap((float)X, (float)Y);
        }
        public Vector2 ScreenToMap(float X, float Y)
        {
            Vector2 mapPosition = new Vector2();
            mapPosition.X = (X - Viewport.Center.X) + Camera.X;
            mapPosition.Y = (Y - Viewport.Center.Y) + Camera.Y;
            return mapPosition;
        }

        protected internal override void LoadContent()
        {
            throw new NotImplementedException();
        }
        protected internal override void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
