using Data;
using LegendWorld.Data.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    [KnownType(typeof(AbilityScrollItem))]
    [DataContract]
    public abstract class Item
    {
        //public int Id { get; set; }
        [Key]
        public int Id { get; set; }
        public ItemIdentity Identity { get; set; }
        public ItemCategory Category { get; set; }

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
