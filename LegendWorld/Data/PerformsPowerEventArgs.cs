using LegendWorld.Data;
using Microsoft.Xna.Framework;

namespace Data.World
{
    public class PerformsPowerEventArgs
    {
        private CharacterPower characterPower;

        public PerformsPowerEventArgs(CharacterPower characterPower)
        {
            this.CharacterPower = characterPower;
        }

        public CharacterPower CharacterPower
        {
            get
            {
                return characterPower;
            }

            protected set
            {
                characterPower = value;
            }
        }
        
    }
}