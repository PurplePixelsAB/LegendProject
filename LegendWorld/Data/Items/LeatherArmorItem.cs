﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Items
{
    public class LeatherArmorItem : ArmorItem
    {
        public LeatherArmorItem()
        {
            Armor = 70;
            Weight = 5000;
        }
    }
}