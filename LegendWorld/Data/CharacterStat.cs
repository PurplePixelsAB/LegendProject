using Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public class CharacterStat
    {
        public CharacterStat(StatIdentifier id)
        {
            this.StatID = id;
        }

        public StatIdentifier StatID { get; private set; }

        private int statValue = 0;
        public int Value { get { return statValue; } set { statValue = value; } }

        public event StatReadEventHandler Read;
        public event StatChangedEventHandler Changed;
        internal StatChangedEventArgs OnStatChanged(StatIdentifier statID, StatChangedEventArgs e, Character character)
        {
            //if (onStatModified[(int)statID] != null)
            //{
            //    onStatModified[(int)statID](character, e);
            //}
            if (this.Changed != null)
            {
                this.Changed(character, e);
            }
            return e;
        }
        internal StatReadEventArgs OnStatRead(StatIdentifier statID, StatReadEventArgs e, Character character)
        {
            //if (onStatRead[(int)statID] != null)
            //{
            //    onStatRead[(int)statID](character, e);
            //}
            if (this.Read != null)
            {
                this.Read(character, e);
            }
            return e;
        }
    }
}
