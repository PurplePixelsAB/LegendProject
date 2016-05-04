using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Abilities
{
    public enum AbilityIdentity : ushort
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
        DamageToEnergy, //Absorb dmg to energy
        Deflect, //Deflect abilities
        Interrupt, //Interrupt abilites
        IncreaseMaxWeight, //Carry more/faster
        IncreaseMaxHealth, //More health
        IncreaseMaxEnergy, //More energy
        IncreaseHealthRegen, //More hp regen
        IncreaseEnergyRegen, //More en regen
        IncreaseSwordPower, //Increase dmg of all attacks with sword
        IncreaseBowPower, //Increase dmg of all attacks with bow
        IncreasePlateArmor,
        IncreaseLeatherArmor,
        IncreaseSpeed,
        ShortSpeedBurst,
    }
}
