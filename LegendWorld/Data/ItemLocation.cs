using System;
using LegendWorld.Data.Items;
using Microsoft.Xna.Framework;

namespace LegendWorld.Data
{
    public class ItemLocation
    {
        public Point? WorldPosition { get; set; }
        public int? WorldMapId { get; set; }
        public bool IsInWorld {  get { return WorldMapId.HasValue; } }


        public int? BagId { get; set; }

        public bool IsInBag {  get { return BagId.HasValue; } }

        internal void SetLocation(BagItem bagItem)
        {
            this.WorldMapId = null;
            this.WorldPosition = null;
            this.BagId = bagItem.Id;
        }

        internal void SetLocation(int worldMapId, Point worldMapPoint)
        {
            this.BagId = null;
            this.WorldPosition = worldMapPoint;
            this.WorldMapId = worldMapId;
        }
    }
}