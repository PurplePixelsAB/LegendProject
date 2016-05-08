using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    [DataContract]
    public class BagItem : ContainerItem //Container
    {
        public BagItem()
        {
            this.Identity = ItemIdentity.Bag;
        }
    }
}
