﻿using Data.World;
using LegendWorld.Data.Modifiers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public delegate void StatReadEventHandler(Character character, StatReadEventArgs e);
    public delegate void StatChangedEventHandler(Character character, StatChangedEventArgs e);
    public class Stats
    {        
        private static readonly int MaxValue = byte.MaxValue;
        private static readonly int MinValue = byte.MinValue;
        public static StatIdentifier[] All = (StatIdentifier[])Enum.GetValues(typeof(StatIdentifier));
        public static int Factor(int baseValue, float modifyAmount)
        {
            //float baseValue = (float)baseStats[statId];
            float modValue = baseValue * modifyAmount;
            int roundedClampedModValue = Stats.Clamp((int)Math.Round(modValue)); //, Stats.MinValue, Stats.MaxValue);
            //this.Set(statId, roundedClampedModValue);
            return roundedClampedModValue;
        }
        public static int Clamp(int value)
        {
            return MathHelper.Clamp(value, Stats.MinValue, Stats.MaxValue);
        }

        private const float baseMovement = 8f;
        private const int baseHealthRegen = 1;
        private const int baseEnergyRegen = 2;

        private Dictionary<StatIdentifier, int> baseStats;
        //private Dictionary<StatIdentifier, int> modStats;
        private Character character;

        private StatChangedEventHandler[] onStatModified = new StatChangedEventHandler[Stats.All.Length];
        private StatReadEventHandler[] onStatRead = new StatReadEventHandler[Stats.All.Length];
        public int Health
        {
            get
            {
                return this.GetStat(StatIdentifier.Health);
            }

            set
            {
                var oldHp = this.GetStat(StatIdentifier.Health); //ToDo: Move to Stats.Update() routine. Move Event to Stats.
                if (oldHp != value)
                {
                    this.Set(StatIdentifier.Health, value);
                    //this.OnHealthChange(oldHp);
                }
            }
        }

        public int Energy
        {
            get
            {
                return this.GetStat(StatIdentifier.Energy);
            }

            set
            {
                //var oldEnergy = this.Stats.GetStat(StatIdentifier.Energy); //ToDo: Move to Stats.Update() routine. Move Event to Stats.
                this.Set(StatIdentifier.Energy, value);
                //this.OnEnergyChange(oldEnergy); //ToDo: Add to Stats as event.
            }
        }
        public int MaxHealth { get { return this.GetStat(StatIdentifier.HealthMax); } }

        public int MaxEnergy { get { return this.GetStat(StatIdentifier.EnergyMax); } }

        public ModifiersCollection Modifiers { get; set; }
        public int HealthRegen { get { return this.GetStat(StatIdentifier.HealthRegeneration); } }
        public int EnergyRegen { get { return this.GetStat(StatIdentifier.EnergyRegeneration); } }

        public Stats(Character attachedCharacter)
        {
            character = attachedCharacter;
            baseStats = new Dictionary<StatIdentifier, int>(Stats.All.Length);
            //modStats = new Dictionary<StatIdentifier, int>(Stats.All.Length);
            int defaultValue = 100;
            foreach (StatIdentifier stat in Stats.All)
            {
                baseStats.Add(stat, defaultValue);
                //modStats.Add(stat, defaultValue);
            }

            this.Set(StatIdentifier.HealthRegeneration, baseHealthRegen);
            this.Set(StatIdentifier.EnergyRegeneration, baseEnergyRegen);
            this.Set(StatIdentifier.Power, 0);
            this.Set(StatIdentifier.Armor, 0);

            Modifiers = new ModifiersCollection(this);
        }

        public void Update(GameTime gameTime)
        {
            int characterWeight = character.GetCarryWeight();
            float maxWeightFactor = this.GetStatFactor(StatIdentifier.MaxWeight);
            int maxWeight = (int)(Character.MaxWeight * maxWeightFactor);
            float weightFact = ((float)characterWeight / (float)maxWeight);
            float weightFactor = 1f - (weightFact * weightFact);
            int mobility = Stats.Clamp((int)Math.Round(100f * weightFactor));
            this.Set(StatIdentifier.Mobility, mobility);
            //this.SetFactor(StatIdentifier.Mobility, MathHelper.Clamp(1f - weightFactor, 0f, 1f), 100);

        }

        internal void SetPower(int power, int weaponPower)
        {
            this.Set(StatIdentifier.Power, power + weaponPower);
        }

        public void OnStatChangedRegister(StatIdentifier statID, StatChangedEventHandler function)
        {
            onStatModified[(int)statID] += function;
        }
        public void OnStatChangedUnRegister(StatIdentifier statID, StatChangedEventHandler function)
        {
            onStatModified[(int)statID] -= function;
        }
        public void OnStatReadRegister(StatIdentifier statID, StatReadEventHandler function)
        {
            onStatRead[(int)statID] += function;
        }
        public void OnStatReadUnRegister(StatIdentifier statID, StatReadEventHandler function)
        {
            onStatRead[(int)statID] -= function;
        }

        private StatChangedEventArgs OnStatChanged(StatIdentifier statID, StatChangedEventArgs e)
        {
            if (onStatModified[(int)statID] != null)
            {
                onStatModified[(int)statID](character, e);
            }
            return e;
        }
        private StatReadEventArgs OnStatRead(StatIdentifier statID, StatReadEventArgs e)
        {
            if (onStatRead[(int)statID] != null)
            {
                onStatRead[(int)statID](character, e);
            }
            return e;
        }

        //public event EventHandler<StatModifyEventArgs> StatModify;
        //protected virtual int OnStatModify(Character character, StatIdentifier statToModify, int newValue, int oldValue)
        //{
        //    int returnValue = newValue;
        //    foreach (CharacterModifier modifier in this.Modifiers)
        //    {
        //        returnValue = modifier.Modify(character, statToModify, returnValue, oldValue);
        //    }

        //    return returnValue;
        //}

        private void Add(StatIdentifier statId, int modifyAmount)
        {
            int baseValue = (int)baseStats[statId];
            int modValue = baseValue + modifyAmount;
            int clampedModValue = MathHelper.Clamp(modValue, Stats.MinValue, Stats.MaxValue);
            this.Set(statId, clampedModValue);
        }
        private void Set(StatIdentifier statId, int newValue)
        {
            int prevValue = this.GetStat(statId);
            var eventArgs = this.OnStatChanged(statId, new StatChangedEventArgs(newValue, prevValue));
            baseStats[statId] = eventArgs.Value;
            //modStats[statId] = this.OnStatModify(character, statId, modifyAmountTo, modStats[statId]);
        }
        //private void SetFactor(StatIdentifier statId, float factor, int restPoint)
        //{
        //    if (factor <= 0f)
        //    {
        //        this.Set(statId, 0);
        //        return;
        //    }

        //    int factorValue = MathHelper.Clamp((int)Math.Round(restPoint * factor), Stats.MinValue, Stats.MaxValue);
        //    this.Set(statId, factorValue);
        //}

        internal int GetStat(StatIdentifier statId)
        {
            int currentValue = baseStats[statId];
            var eventArgs = this.OnStatRead(statId, new StatReadEventArgs(currentValue));
            return eventArgs.Value;
            //return modStats[statId];
        }
        private float GetStatFactor(StatIdentifier statId)
        {
            return GetStatFactor(statId, 100);
        }
        private float GetStatFactor(StatIdentifier statId, int restPoint)
        {
            int statValue = this.GetStat(statId);
            if (statValue <= 0)
                return 0f;
            if (restPoint <= 0)
                return 0f;

            return (float)statValue / (float)restPoint;
        }
        //private int GetModdedStatByFactor(StatIdentifier statId, int baseValue)
        //{
        //    if (baseValue == 0)
        //        return 0;

        //    float factor = this.GetStatFactor(statId);
        //    int moddedValue = MathHelper.Clamp((int)Math.Round(baseValue * factor), Stats.MinValue, Stats.MaxValue);
        //    return moddedValue;
        //}

        internal bool HasModifier(Type modifierType)
        {
            foreach (var modifer in this.Modifiers)
            {
                if (modifer.GetType() == modifierType)
                    return true;
            }

            return false;
        }

        internal int GetWeaponPower()
        {
            int weaponPower = 0;

            if (this.character.LeftHand != null)
                weaponPower += this.character.LeftHand.Power;

            if (this.character.RightHand != null)
                weaponPower += this.character.RightHand.Power;

            return weaponPower;
        }

        internal int CalculateDamageTaken(int attackersPower)
        {
            float defenderArmor = 0f;
            if (character.Armor != null)
                defenderArmor = (int)character.Armor.Armor;

            float armorFactor = this.GetStatFactor(StatIdentifier.Armor, 255);
            float moddedArmor = defenderArmor * (1f + armorFactor);
            int damageTaken = (int)MathHelper.Clamp(Stats.Factor(attackersPower, 1f - moddedArmor), 1, Stats.MaxValue);

            return damageTaken;
        }

        //internal int CalculateAbilityPower(int power)
        //{
        //    float powerFactor = this.GetStatFactor(StatIdentifier.Power);
        //    return this.Factor(power, powerFactor);
        //}
        public float GetVisibility(float distance)
        {
            if (distance == 0f)
                distance = 10f;

            float visibleDistance = 50f;
            //float farDistance = 1000f;
            float minVisibility = 0f;
            float maxVisibility = 1f;

            if (distance <= visibleDistance)
                minVisibility = .3f;            

            float visibilityFactor = this.GetStatFactor(StatIdentifier.Visibility);
            float distanceFactor = distance / (visibleDistance * 2f);

            if (visibilityFactor < 1f - minVisibility)
                maxVisibility = minVisibility + visibilityFactor;

            //float distanceFactor = (farDistance * visibilityFactor) / distance;
            var distanceLerpValue = MathHelper.Clamp((float)Math.Pow(visibilityFactor, distanceFactor), 0f, 1f);
            var lerpVisibility = MathHelper.Lerp(minVisibility, maxVisibility, distanceLerpValue);

            return lerpVisibility;
        }
        internal Vector2 CalculateMovement(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                return Vector2.Zero;

            //Vector2 movement = direction * ((float)this.GetStat(StatIdentifier.MovementSpeed) * .1f);
            float mobilityFactor = this.GetStatFactor(StatIdentifier.Mobility);
            Vector2 movement = (direction * baseMovement) * mobilityFactor; //((float)this.GetStat(StatIdentifier.MovementSpeed) * .1f);
            return movement;
        }
        internal int CalculateEnergyCost(int baseEnergyCost)
        {
            float ecFactor = this.GetStatFactor(StatIdentifier.EnergyCost);
            return Stats.Factor(baseEnergyCost, ecFactor);
        }

        public override string ToString()
        {
            string listText = string.Empty;
            foreach (var stat in Stats.All)
            {
                listText += string.Format("{0}: {1}{2}", stat.ToString(), this.GetStat(stat), Environment.NewLine);
            }

            return listText;
        }

        //public class StatModifyEventArgs : EventArgs
        //{
        //    private int newValue;
        //    private int oldValue;
        //    private StatIdentifier statToModify;

        //    public StatModifyEventArgs(StatIdentifier statToModify, int oldValue, int newValue)
        //    {
        //        this.statToModify = statToModify;
        //        this.oldValue = oldValue;
        //        this.newValue = newValue;
        //    }

        //    public int NewValue
        //    {
        //        get
        //        {
        //            return newValue;
        //        }

        //        set
        //        {
        //            newValue = value;
        //        }
        //    }

        //    public int OldValue
        //    {
        //        get
        //        {
        //            return oldValue;
        //        }

        //        set
        //        {
        //            oldValue = value;
        //        }
        //    }

        //    public StatIdentifier Stat
        //    {
        //        get
        //        {
        //            return statToModify;
        //        }

        //        set
        //        {
        //            statToModify = value;
        //        }
        //    }
        ////}
    }
    public class StatChangedEventArgs : EventArgs
    {
        public StatChangedEventArgs(int value, int prevValue)
        {
            this.Value = value;
            this.PreviousValue = prevValue;
        }

        public int Value { get; set; }
        public int PreviousValue { get; private set; }
    }
    public class StatReadEventArgs : EventArgs
    {
        public StatReadEventArgs(int value)
        {
            this.Value = value;
        }

        public int Value { get; set; }
    }
}
