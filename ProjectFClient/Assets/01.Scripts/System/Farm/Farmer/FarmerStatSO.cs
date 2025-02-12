using System;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Stats;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    [CreateAssetMenu(menuName = "SO/Farm/FarmerStat")]
    public class FarmerStatSO : DataTableSO<FarmerTable, FarmerTableRow>
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

        protected override void OnTableInitialized()
        {
            base.OnTableInitialized();

            statDictionary = new Dictionary<EFarmerStatType, Stat>();
            
            AddStat(EFarmerStatType.MoveSpeed, TableRow.moveSpeed);
            AddStat(EFarmerStatType.IQ, TableRow.iq);
            AddStat(EFarmerStatType.Strength, TableRow.strength);
            AddStat(EFarmerStatType.Luck, TableRow.luck);
            AddStat(EFarmerStatType.FarmingSkill, TableRow.farmingSkill);
            AddStat(EFarmerStatType.HarvestingSkill, TableRow.harvestingSkill);

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

        /// <summary>
        /// 2025.02.11 SetBaseValue 추가. baseValue 수정 기능
        /// </summary>
        public void SetBaseValue(EFarmerStatType statType, float value)
        {
            this[statType]?.SetBaseValue(value);
        }
    }
}