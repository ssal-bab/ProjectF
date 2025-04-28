using System;
using System.Collections.Generic;
using H00N.Stats;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    [Serializable]
    public class FarmerStat
    {
        private Dictionary<EFarmerStatType, Stat> statDictionary;
        public Stat this[EFarmerStatType indexer]
        {
            get
            {
                if (statDictionary.ContainsKey(indexer) == false)
                {
                    Debug.LogWarning("Stat of Given Type is Doesn't Existed");
                    return null;
                }

                return statDictionary[indexer];
            }
        }

        public event Action OnStatChangedEvent = null;

        private float currentHP = 0f;
        public float CurrentHP => currentHP;

        public FarmerStat()
        {
            statDictionary = new Dictionary<EFarmerStatType, Stat>();
        }

        public void SetData(FarmerLevelTableRow tableRow)
        {
            UpdateStat(EFarmerStatType.MoveSpeed, tableRow.moveSpeed);
            UpdateStat(EFarmerStatType.Health, tableRow.health);
            UpdateStat(EFarmerStatType.FarmingSkill, tableRow.farmingSkill);
            UpdateStat(EFarmerStatType.AdventureSkill, tableRow.adventureSkill);
            SetHP(this[EFarmerStatType.Health], false);
            OnStatChangedEvent?.Invoke();
        }

        private void UpdateStat(EFarmerStatType statType, float baseValue)
        {
            if(statDictionary.TryGetValue(statType, out Stat stat) == false)
            {
                stat = new Stat();
                stat.Initialize(0);
                statDictionary.Add(statType, stat);
            }

            stat.SetBaseValue(baseValue);
        }

        public void AddModifier(EFarmerStatType statType, EStatModifierType modifierType, float value)
        {
            this[statType]?.AddModifier(modifierType, value);
            OnStatChangedEvent?.Invoke();
        }

        public void RemoveModifier(EFarmerStatType statType, EStatModifierType modifierType, float value)
        {
            this[statType]?.RemoveModifier(modifierType, value);
            OnStatChangedEvent?.Invoke();
        }

        public void ReduceHP(float amount)
        {
            SetHP(CurrentHP - amount);
        }

        public void IncreaseHP(float amount)
        {
            SetHP(CurrentHP + amount);
        }

        private void SetHP(float value, bool notify = true)
        {
            currentHP = Mathf.Clamp(value, 0, this[EFarmerStatType.Health]);
            if(notify)
                OnStatChangedEvent?.Invoke();
        }
    }
}
