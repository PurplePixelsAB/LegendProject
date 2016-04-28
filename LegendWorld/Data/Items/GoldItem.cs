using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class GoldItem : StackableItem
    {
        public GoldItem()
        {
            this.Identity = ItemIdentity.Gold;
            this.StackCount = 1;
        }
    }
}
