using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public class GoldItem : StackableItem
    {
        public GoldItem()
        {
            this.Identity = ItemIdentity.Gold;
            this.Weight = 1;
        }
    }
}
