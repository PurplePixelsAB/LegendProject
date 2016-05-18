using LegendWorld.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LegendClient.World.Items
{
    public class GoldClientItem : GoldItem, IClientItem
    {
        public Texture2D Texture { get; set; }
    }
}
