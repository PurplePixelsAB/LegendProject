using LegendWorld.Data.Items;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.World.Items
{
    public class LeatherArmorClientItem : LeatherArmorItem, IArmorClientItem
    {
        public Texture2D Texture { get; set; }
        public Texture2D HeadTexture { get; set; }
    }
}
