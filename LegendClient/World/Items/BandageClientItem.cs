using System;
using Data;
using LegendClient.World.Items;
using LegendWorld.Data.Items;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsClient.World
{
    internal class BandageClientItem : BandageItem, IClientItem
    {
        public Texture2D Texture { get; set; }
    }
}