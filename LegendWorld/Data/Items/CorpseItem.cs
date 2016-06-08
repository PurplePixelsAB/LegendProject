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

        public int CharacterID { get { return this.SubType.HasValue ? this.SubType.Value : 0; } set { this.SubType = value; } }
    }
}