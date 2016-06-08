using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    //[JsonConverter(typeof(ItemJsonConverter))]
    [DataContract]
    public class ItemModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Identity { get; set; }

        [DataMember]
        public int? SubType { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int? WorldMapId { get; set; }

        [DataMember]
        public int? WorldX { get; set; }

        [DataMember]
        public int? WorldY { get; set; }

        
        [DataMember]
        public int? ContainerId { get; set; }

        //public void MoveTo(int mapId, Point worldLocation)
        //{
        //    //this.Container = null;
        //    this.ContainerId = null;

        //    this.WorldMapId = mapId;
        //    this.WorldX = worldLocation.X;
        //    this.WorldY = worldLocation.Y;
        //}
        //public void MoveTo(int containerId)//(ItemModel containerItem)
        //{
        //    this.WorldMapId = null;
        //    this.WorldX = null;
        //    this.WorldY = null;

        //    //this.Container = containerItem;
        //    this.ContainerId = containerId;
        //}

        //public enum ItemIdentity : int
        //{
        //    Bag,
        //    Gold,
        //    LeatherArmor,
        //    PlateArmor,
        //    ClothRobe,
        //    Dagger,
        //    Sword,
        //    Bow,
        //    PowerScoll,
        //    Bandage,
        //    Corpse
        //}
    }
}
