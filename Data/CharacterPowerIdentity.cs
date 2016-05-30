using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public enum CharacterPowerIdentity : int
    {
        DefaultAttack, //Everybody Has
        HardAttack, //No Movement extra Dmg
        CriticalAttack, //Increase dmg of next ability
        StunAttack,
        SlowingAttack,
        DecreaseEnergyCost, //Decrease energy cost of DefaultAttack
        IncreaseEnergyCost, //Increase energy cost of DefaultAttack
        DecreaseDuration, //Decrease duration of DefaultAttack
        IncreaseDuration, //Increase duration of DefaultAttack
        Meditation, //Increase EnergyRegen active use
        AbsorbDamage, //Absorb dmg to energy
        DeflectDamage, //Deflect abilities
        ShortSpeedBurst,
        IncreaseSpeed,
        IncreaseMaxHealth, //More health
        IncreaseMaxEnergy, //More energy
        IncreaseHealthRegen, //More hp regen
        IncreaseEnergyRegen, //More en regen
        Stealth,
        IncreaseSwordPower, //Increase dmg of all attacks with sword
        IncreaseBowPower, //Increase dmg of all attacks with bow
        IncreaseDaggerPower, //Increase dmg of all attacks with dagger
        IncreasePlateArmor,
        IncreaseLeatherArmor,
        Resurrect,
        //Comeplete

        //ToDo
        IncreaseMaxWeight, //Carry more/faster
        Interrupt, //Interrupt abilites
    }
}
