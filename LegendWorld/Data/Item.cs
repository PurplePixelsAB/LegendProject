using Data;
using LegendWorld.Data.Items;
using LegendWorld.Network;
using Newtonsoft.Json;
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

    public interface IItem
    {
        ItemData Data { get; set; }
        //public int Id { get; set; }
        
        //ItemData.ItemIdentity Identity { get { return this.Data.Identity; } }
        
        ItemCategory Category { get; }
        
        int Weight { get; }

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
