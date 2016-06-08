using System;
using System.Runtime.Serialization;
using Data;

namespace LegendWorld.Data.Items
{
    public class CorpseItem : ContainerItem
    {
        public CorpseItem()
        {
            this.Identity = ItemIdentity.Corpse;

            this.Weight = 80000;
            this.Count = 1;
        }

        public int CharacterID { get { return this.SubType.HasValue ? this.SubType.Value : 0; } set { this.SubType = value; } }
    }
}