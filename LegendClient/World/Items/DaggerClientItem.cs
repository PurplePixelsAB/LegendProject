using LegendWorld.Data.Items;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendClient.World.Items
{
    public class DaggerClientItem : DaggerItem, IClientItem
    {
        public Texture2D Texture { get; set; }
    }
}
