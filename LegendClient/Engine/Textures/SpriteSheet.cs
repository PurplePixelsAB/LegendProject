using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine.Textures
{
    public class SpriteSheet
    {
        public SpriteSheet(SpriteSheetData data)
        {
            this.Texture = data.Texture;
            foreach (string name in data.SpriteNames.Keys)
            {
                Sprite sprite = new Sprite(this, name, data.SpriteRectangles[data.SpriteNames[name]]);
                this.Sprites.Add(name, sprite);
            }
        }
        Texture2D texture = null;

        public Texture2D Texture { get { return texture; } private set { texture = value; } }

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public Dictionary<string, Sprite> Sprites { get { return sprites; } }
        public int Count { get { return sprites.Count; } }

        public Sprite GetSprite(string SpriteName)
        {
            return sprites[SpriteName];
        }

        public Rectangle GetSourceRectangle(string SpriteName)
        {
            return this.GetSprite(SpriteName).Source;
        }
    }

    public class SpriteSheetData
    {
        // Single texture contains many separate sprite images.
        [ContentSerializer]
        public Texture2D Texture = null;

        // Remember where in the texture each sprite has been placed.
        [ContentSerializer]
        public List<Rectangle> SpriteRectangles = new List<Rectangle>();

        // Store the original sprite filenames, so we can look up sprites by name.
        [ContentSerializer]
        public Dictionary<string, int> SpriteNames = new Dictionary<string, int>();
    }

    



}
