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

        public int CharacterID { get { return this.Data.SubType.HasValue ? this.Data.SubType.Value : 0; } set { this.Data.SubType = value; } }
    }
}