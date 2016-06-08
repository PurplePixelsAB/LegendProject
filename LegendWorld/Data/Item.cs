using Data;
using LegendWorld.Data.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public abstract class Item : IItem
    {
        public int Id { get; private set; }
        public ItemCategory Category { get; protected set; }
        public ItemIdentity Identity { get; protected set; }
        public int? SubType { get; protected set; }

        public int Weight { get; protected set; }

        public int? WorldMapId { get; private set; }
        public int? WorldX { get; private set; }
        public int? WorldY { get; private set; }

        public int? ContainerId { get; private set; }

        public virtual void LoadData(ItemModel data)
        {
            this.Id = data.Id;
            this.SubType = data.SubType;
            this.WorldMapId = data.WorldMapId;
            this.WorldX = data.WorldX;
            this.WorldY = data.WorldY;
            this.ContainerId = data.ContainerId;
        }
        public abstract int GetTotalWeight();        
        public bool IsWorldItem {  get { return this.WorldMapId.HasValue && this.WorldX.HasValue && this.WorldY.HasValue; } }        
        public Point WorldLocation { get { return this.IsWorldItem ? new Point(this.WorldX.Value, this.WorldY.Value) : Point.Zero; } }

        public void MoveTo(ContainerItem container)
        {
            if (container.Id == this.Id)
                return; //Not Allowed to be placed within itself.

            this.WorldMapId = this.WorldX = this.WorldY = null;
            this.ContainerId = container.Id;
        }
        public void MoveTo(int mapId, Point mapPosition)
        {
            this.ContainerId = null;
            this.WorldMapId = mapId;
            this.WorldX = mapPosition.X;
            this.WorldY = mapPosition.Y;
        }
        internal void Remove()
        {
            this.ContainerId = null;
            this.WorldMapId = null;
            this.WorldX = null;
            this.WorldY = null;
        }
    }


    public interface IItem
    {
        //ItemModel Data { get; set; }
        int Id { get; }

        ItemIdentity Identity { get ; }

        ItemCategory Category { get; }
        
        int Weight { get; }

        int GetTotalWeight();
        
        int? WorldMapId { get; }
        
        int? WorldX { get; }
        
        int? WorldY { get; }

        int? ContainerId { get; }

        //public virtual string GetInventoryString()
        //{
        //    if (this.Data.IsStackable)
        //    {
        //        return string.Format("{1} {0}", this.Data.Name, this.StackCount);
        //    }
        //    else
        //        return this.Data.Name;
        //}

        //public void OnUse()
        //{

        //}

    }
}
