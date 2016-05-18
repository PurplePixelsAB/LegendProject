using LegendWorld.Data;

namespace Data.World
{
    public class PrepareAbility
    { 
        public PrepareAbility(CharacterPower abilityPrepare)
        {
            this.Ability = abilityPrepare;
        }
    
        public CharacterPower Ability { get; private set; }
        public int PrepareDuration { get { return this.Ability.PrepareTime; } }
        public int Elapsed { get; set; }
    }
}