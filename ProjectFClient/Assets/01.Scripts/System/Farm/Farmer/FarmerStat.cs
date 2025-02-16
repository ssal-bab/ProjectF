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

        public FarmerStat(FarmerStatTableRow tableRow)
        {
            statDictionary = new Dictionary<EFarmerStatType, Stat>();

            AddStat(EFarmerStatType.MoveSpeed, tableRow.moveSpeedBaseValue);
            AddStat(EFarmerStatType.Health, tableRow.healthBaseValue);
            AddStat(EFarmerStatType.FarmingSkill, tableRow.farmingSkillBaseValue);
            AddStat(EFarmerStatType.AdventureSkill, tableRow.adventureSkillBaseValue);

            OnStatChangedEvent?.Invoke();
        }

        private void AddStat(EFarmerStatType statType, float baseValue)
        {
            statDictionary.Add(statType, new Stat());
            statDictionary[statType].Initialize(baseValue);
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
