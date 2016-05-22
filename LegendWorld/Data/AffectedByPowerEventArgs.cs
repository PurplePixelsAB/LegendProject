using LegendWorld.Data;

namespace Data.World
{
    public class AffectedByPowerEventArgs
    {
        private Character abilityPerformedBy;
        private CharacterPower characterPower;

        public AffectedByPowerEventArgs(CharacterPower characterPower, Character abilityPerformedBy)
        {
            this.characterPower = characterPower;
            this.abilityPerformedBy = abilityPerformedBy;
        }
    }
}