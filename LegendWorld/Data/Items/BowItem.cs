﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class BowItem : WeaponItem
    {
        public BowItem()
        {
            this.Identity = ItemIdentity.Bow;

            this.Power = 15;
            this.Speed = 75;
            this.Weight = 2000;
            this.IsTwoHanded = true;
            this.Count = 1;
        }
    }
}
