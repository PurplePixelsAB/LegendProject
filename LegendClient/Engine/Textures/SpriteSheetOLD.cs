using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine.Textures
{
    /// <summary>
    /// A sprite sheet contains many individual sprite images, packed into different
    /// areas of a single larger texture, along with information describing where in
    /// that texture each sprite is located. Sprite sheets can make your game drawing
    /// more efficient, because they reduce the number of times the graphics hardware
    /// needs to switch from one texture to another.
    /// </summary>
    public class RawSpriteSheet
    {
        // Single texture contains many separate sprite images.
        [ContentSerializer]
        Texture2D texture = null;

        // Remember where in the texture each sprite has been placed.
        [ContentSerializer]
        List<Rectangle> spriteRectangles = null;

        // Store the original sprite filenames, so we can look up sprites by name.
        [ContentSerializer]
        Dictionary<string, int> spriteNames = null;


        /// <summary>
        /// Gets the single large texture used by this sprite sheet.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }


        /// <summary>
        /// Looks up the location of the specified sprite within the big texture.
        /// </summary>
        public Rectangle SourceRectangle(string spriteName)
        {
            int spriteIndex = GetIndex(spriteName);

            return spriteRectangles[spriteIndex];
        }


        /// <summary>
        /// Looks up the location of the specified sprite within the big texture.
        /// </summary>
        public Rectangle SourceRectangle(int spriteIndex)
        {
            if ((spriteIndex < 0) || (spriteIndex >= spriteRectangles.Count))
                throw new ArgumentOutOfRangeException("spriteIndex");

            return spriteRectangles[spriteIndex];
        }


        /// <summary>
        /// Looks up the numeric index of the specified sprite. This is useful when
        /// implementing animation by cycling through a series of related sprites.
        /// </summary>
        public int GetIndex(string spriteName)
        {
            int index;

            if (!spriteNames.TryGetValue(spriteName, out index))
            {
                string error = "SpriteSheet does not contain a sprite named '{0}'.";

                throw new KeyNotFoundException(string.Format(error, spriteName));
            }

            return index;
        }

        public int Count { get { return spriteRectangles.Count; } }
    }

    public class SpriteSheet
    {
        public SpriteSheet(RawSpriteSheet data)
        {
            this.Load(data);
        }
        public SpriteSheet()
        {

        }

        Dictionary<string, Sprite> Sprites = null;

        public void Load(RawSpriteSheet data)
        {
            Sprites = new Dictionary<string, Sprite>();

            for (int i = 0; i < data.Count; i++)
            {
                Sprite sprite = new Sprite();
                sprite.Source = data.SourceRectangle(i);
                sprite.Name = data.
            }
        }
    }
}
