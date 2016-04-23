using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Textures
{
    public class OnePixelTexture : Texture2D
    {
        public OnePixelTexture(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, 1, 1)
        {
            this.SetData<Color>(new Color[1] { Color.White });
        }
        public OnePixelTexture(GraphicsDevice graphicsDevice, Color pixelColor)
            : base(graphicsDevice, 1, 1)
        {
            this.SetData<Color>(new Color[1] { pixelColor });
        }
    }
}
