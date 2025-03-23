using System;
using System.Collections.Generic;
using H00N.Stats;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
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

        public FarmerStat()
        {
            statDictionary = new Dictionary<EFarmerStatType, Stat>();
        }

        public void SetData(FarmerStatTableRow tableRow, int level)
        {
            UpdateStat(EFarmerStatType.MoveSpeed, new CalculateStat(tableRow.moveSpeed, level).currentStat);
            UpdateStat(EFarmerStatType.Health, new CalculateStat(tableRow.health, level).currentStat);
            UpdateStat(EFarmerStatType.FarmingSkill, new CalculateStat(tableRow.farmingSkill, level).currentStat);
            UpdateStat(EFarmerStatType.AdventureSkill, new CalculateStat(tableRow.adventureSkill, level).currentStat);
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
    }
}
