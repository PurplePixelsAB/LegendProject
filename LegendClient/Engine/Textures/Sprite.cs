using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine.Textures
{
    public class Sprite
    {
        public Sprite(SpriteSheet ParentSheet, string SpriteName, Rectangle SourceRectangle)
        {
            sheet = ParentSheet;
            name = SpriteName;
            source = SourceRectangle;
        }
        SpriteSheet sheet = null;

        string name = null;
        public string Name { get { return name; } }

        Rectangle source = Rectangle.Empty;
        public Rectangle Source { get { return source; } }

        public Texture2D Texture { get { return sheet.Texture; } }
    }
}
