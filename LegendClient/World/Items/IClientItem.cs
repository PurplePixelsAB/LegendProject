using LegendWorld.Data;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.World.Items
{
    public interface IClientItem : IItem
    {
        Texture2D Texture { get; set; }
    }
    public interface IArmorClientItem : IClientItem
    {
        Texture2D HeadTexture { get; set; }
    }
}
