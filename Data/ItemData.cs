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
    public class ItemData
    {
        [Key]
        [DataMember]
        public int ItemDataID { get; set; }

        [DataMember]
        public ItemIdentity Identity { get; set; }

        [DataMember]
        public int? SubType { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int? WorldMapID { get; set; }

        [DataMember]
        public int? WorldX { get; set; }

        [DataMember]
        public int? WorldY { get; set; }

        [NotMapped]
        public bool IsWorldItem {  get { return this.WorldMapID.HasValue && this.WorldX.HasValue && this.WorldY.HasValue; } }
        
        [NotMapped]
        public Point WorldLocation { get { return this.IsWorldItem ? new Point(this.WorldX.Value, this.WorldY.Value) : Point.Zero; } }


        [DataMember]
        [ForeignKey("Container")]
        public int? ContainerID { get; set; }

        [DataMember]
        public virtual ItemData Container { get; set; }

        public void MoveTo(int mapId, Point worldLocation)
        {
            this.Container = null;
            this.ContainerID = null;

            this.WorldMapID = mapId;
            this.WorldX = worldLocation.X;
            this.WorldY = worldLocation.Y;
        }
        public void MoveTo(ItemData containerItem)
        {
            this.WorldMapID = null;
            this.WorldX = null;
            this.WorldY = null;

            this.Container = containerItem;
            this.ContainerID = containerItem.ItemDataID;
        }

        public enum ItemIdentity : int
        {
            Bag,
            Gold,
            LeatherArmor,
            PlateArmor,
            ClothRobe,
            Dagger,
            Sword,
            Bow,
            PowerScoll,
            Bandage
        }
    }
}
