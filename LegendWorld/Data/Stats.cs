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
        private Dictionary<StatIdentifier, byte> baseStats;
        private Dictionary<StatIdentifier, byte> modStats;

        public Stats()
        {
            baseStats = new Dictionary<StatIdentifier, byte>(Stats.All.Length);
            modStats = new Dictionary<StatIdentifier, byte>(Stats.All.Length);
            byte defaultValue = 100;
            foreach (StatIdentifier stat in Stats.All)
            {
                baseStats.Add(stat, defaultValue);
                modStats.Add(stat, defaultValue);
            }
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

        internal void Modify(StatIdentifier statId, float modifyAmount)
        {
            float baseValue = (float)baseStats[statId];
            float modValue = baseValue * modifyAmount;
            byte roundedClampedModValue = (byte)MathHelper.Clamp((float)Math.Round(modValue), byte.MinValue, byte.MaxValue);
            this.Modify(statId, roundedClampedModValue);
        }

        internal byte CalculateDamageTaken(byte attackersPower)
        {
            byte armor = this.GetStat(StatIdentifier.Armor);
            byte damageTaken = (byte)MathHelper.Clamp(attackersPower - armor, 1, 255);

            return damageTaken;
        }

        internal byte CalculateAbilityPower(byte power)
        {
            return this.GetModdedStatByFactor(StatIdentifier.Power, power);
        }

        internal void Modify(StatIdentifier statId, int modifyAmount)
        {
            int baseValue = (int)baseStats[statId];
            int modValue = baseValue + modifyAmount;
            byte clampedModValue = (byte)MathHelper.Clamp(modValue, byte.MinValue, byte.MaxValue);
            this.Modify(statId, clampedModValue);
        }

        internal void Modify(StatIdentifier statId, byte modifyAmountTo)
        {
            modStats[statId] = modifyAmountTo;
        }

        internal byte GetStat(StatIdentifier statId)
        {
            return modStats[statId];
        }

        internal Vector2 CalculateMovement(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                return Vector2.Zero;

            Vector2 movement = direction * ((float)this.GetStat(StatIdentifier.MovementSpeed) * .2f);
            return movement;
        }
        internal byte CalculateEnergyCost(byte baseEnergyCost)
        {
            return this.GetModdedStatByFactor(StatIdentifier.EnergyCost, baseEnergyCost);
        }

        private float GetStatFactor(StatIdentifier statId)
        {
            return MathHelper.Clamp(modStats[statId], 1f, 255f) / 100f; //(float)byte.MaxValue; //MathHelper.Clamp(baseStats[StatIdentifier.EnergyCost], 1f, 255f);
        }
        private byte GetModdedStatByFactor(StatIdentifier statId, byte baseValue)
        {
            if (baseValue == 0)
                return 0;

            float factor = this.GetStatFactor(statId);
            byte moddedValue = (byte)MathHelper.Clamp((int)Math.Round(baseValue * factor), 1, 255);
            return moddedValue;
        }
    }
}
