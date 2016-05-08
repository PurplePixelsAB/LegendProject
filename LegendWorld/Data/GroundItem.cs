using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegendWorld.Data
{
    [DataContract]
    public class GroundItem //: IHasPosition
    {
        [DataMember]
        public int CurrentMapId { get; set; }

        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public int PositionX { get; set; }
        [DataMember]
        public int PositionY { get; set; }

        [NotMapped]
        public Point Position
        {
            get { return new Point(this.PositionX, this.PositionY); }
            set
            {
                if (value != null)
                {
                    this.PositionX = value.X;
                    this.PositionY = value.Y;
                }
                else
                {
                    this.PositionX = this.PositionY = 0;
                }
            }
        }

        [DataMember]
        public int ItemId { get; set; }
        
    }
}
