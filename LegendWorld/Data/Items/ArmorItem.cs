using System;
using System.Runtime.Serialization;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public abstract class ArmorItem : Item
    {
        public ArmorItem()
        {
            this.Category = ItemCategory.Armor;
        }
        [DataMember]
        public int Armor { get; set; }

        //public ItemCategory Category { get; set; }

        //public ushort Id { get; set; }

        //public ItemIdentity Identity { get; set; }
    }
}