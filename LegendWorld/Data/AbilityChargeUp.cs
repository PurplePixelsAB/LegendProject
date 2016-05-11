using LegendWorld.Data;

namespace Data.World
{
    public class PrepareAbility
    { 
        public PrepareAbility(Ability abilityPrepare)
        {
            this.Ability = abilityPrepare;
        }
    
        public Ability Ability { get; private set; }
        public int PrepareDuration { get { return this.Ability.PrepareTime; } }
        public int Elapsed { get; set; }
    }
}