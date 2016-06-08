using LegendWorld.Data;
using System;

namespace Data.World
{
    public class PowerLearnedEventArgs : EventArgs
    {
        public CharacterPowerIdentity Power { get; set; }
    }
}