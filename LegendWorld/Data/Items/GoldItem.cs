using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class GoldItem : Item
    {
        public GoldItem()
        {
            this.Name = "Gold coin(s)";
            this.IsStackable = true;
            this.StackCount = 1;
            this.Weight = 1;
        }
    }
}
