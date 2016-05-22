using Data.World;
using LegendWorld.Data.Modifiers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public class Stats
    {
        public static StatIdentifier[] All = (StatIdentifier[])Enum.GetValues(typeof(StatIdentifier));
        private Dictionary<StatIdentifier, int> baseStats;
        private Dictionary<StatIdentifier, int> modStats;
        private Character character;

        public ModifiersCollection Modifiers { get; set; }

        public Stats(Character attachedCharacter)
        {
            character = attachedCharacter;
            baseStats = new Dictionary<StatIdentifier, int>(Stats.All.Length);
            modStats = new Dictionary<StatIdentifier, int>(Stats.All.Length);
            byte defaultValue = 100;
            foreach (StatIdentifier stat in Stats.All)
            {
                baseStats.Add(stat, defaultValue);
                modStats.Add(stat, defaultValue);
            }
            Modifiers = new ModifiersCollection();
        }

        public void Update(GameTime gameTime)
        {
            baseStats[StatIdentifier.Health] = modStats[StatIdentifier.Health];
            baseStats[StatIdentifier.Energy] = modStats[StatIdentifier.Energy];
            foreach (StatIdentifier stat in Stats.All)
            {
                modStats[stat] = baseStats[stat];
            }
        }

        //public event EventHandler<StatModifyEventArgs> StatModify;
        protected virtual int OnStatModify(Character character, StatIdentifier statToModify, int newValue, int oldValue)
        {
            int returnValue = newValue;
            foreach (CharacterModifier modifier in this.Modifiers)
            {
                returnValue = modifier.Modify(character, statToModify, returnValue, oldValue);
            }

            return returnValue;
        }

        internal int CalculateDamageTaken(int attackersPower)
        {
            int defenderArmor = 0;
            if (character.Armor != null)
                defenderArmor = (int)character.Armor.Armor;

            int armor = this.GetModdedStatByFactor(StatIdentifier.Armor, defenderArmor);
            int damageTaken = (int)MathHelper.Clamp(attackersPower - armor, 1, 255);

            return damageTaken;
        }

        internal int CalculateAbilityPower(int power)
        {
            return this.GetModdedStatByFactor(StatIdentifier.Power, power);
        }

        public void Modify(StatIdentifier statId, int modifyAmount)
        {
            int baseValue = (int)baseStats[statId];
            int modValue = baseValue + modifyAmount;
            int clampedModValue = MathHelper.Clamp(modValue, byte.MinValue, byte.MaxValue);
            this.Set(statId, clampedModValue);
        }
        public void Set(StatIdentifier statId, int modifyAmountTo)
        {
            modStats[statId] = this.OnStatModify(character, statId, modifyAmountTo, modStats[statId]);
        }
        public void Factor(StatIdentifier statId, float modifyAmount)
        {
            float baseValue = (float)baseStats[statId];
            float modValue = baseValue * modifyAmount;
            int roundedClampedModValue = MathHelper.Clamp((int)Math.Round(modValue), byte.MinValue, byte.MaxValue);
            this.Set(statId, roundedClampedModValue);
        }

        internal bool HasModifier(Type modifierType)
        {
            foreach (var modifer in this.Modifiers)
            {
                if (modifer.GetType() == modifierType)
                    return true;
            }

            return false;
        }
        internal int GetStat(StatIdentifier statId)
        {
            return modStats[statId];
        }

        internal Vector2 CalculateMovement(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                return Vector2.Zero;

            Vector2 movement = direction * ((float)this.GetStat(StatIdentifier.MovementSpeed) * .1f);
            return movement;
        }
        internal int CalculateEnergyCost(int baseEnergyCost)
        {
            return this.GetModdedStatByFactor(StatIdentifier.EnergyCost, baseEnergyCost);
        }

        private float GetStatFactor(StatIdentifier statId)
        {
            return GetStatFactor(statId, 100);
        }
        private float GetStatFactor(StatIdentifier statId, int restPoint)
        {
            float restPointFloat = restPoint;
            return MathHelper.Clamp(modStats[statId], 1f, 255f) / restPointFloat;
        }
        private int GetModdedStatByFactor(StatIdentifier statId, int baseValue)
        {
            if (baseValue == 0)
                return 0;

            float factor = this.GetStatFactor(statId);
            int moddedValue = MathHelper.Clamp((int)Math.Round(baseValue * factor), 1, 255);
            return moddedValue;
        }

        public class StatModifyEventArgs : EventArgs
        {
            private int newValue;
            private int oldValue;
            private StatIdentifier statToModify;

            public StatModifyEventArgs(StatIdentifier statToModify, int oldValue, int newValue)
            {
                this.statToModify = statToModify;
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public int NewValue
            {
                get
                {
                    return newValue;
                }

                set
                {
                    newValue = value;
                }
            }

            public int OldValue
            {
                get
                {
                    return oldValue;
                }

                set
                {
                    oldValue = value;
                }
            }

            public StatIdentifier Stat
            {
                get
                {
                    return statToModify;
                }

                set
                {
                    statToModify = value;
                }
            }
        }
    }
}
