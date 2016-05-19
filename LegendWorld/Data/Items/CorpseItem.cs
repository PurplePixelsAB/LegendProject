using System;
using System.Runtime.Serialization;
using Data;

namespace LegendWorld.Data.Items
{
    public class CorpseItem : ContainerItem
    {
        public CorpseItem()
        {
            this.Weight = 80000;
        }        

        public int CharacterID { get; set; }
        //public BagItem Inventory { get; set; }
        //public ArmorItem Armor { get; set; }

        //public WeaponItem LeftHand { get; set; }

        //public WeaponItem RightHand { get; set; }
    }
}