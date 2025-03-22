using System;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Stats;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Farms;
using UnityEngine;

namespace ProjectF.Farms
{
    [CreateAssetMenu(menuName = "SO/Farm/FarmerIncreaseStat")]
    public class FarmerIncreaseStatSO : DataTableSO<FarmerStatTable, FarmerStatTableRow>
    {
        private Dictionary<EFarmerStatType, float> statDictionary;
        public float this[EFarmerStatType indexer]
        {
            get
            {
                if (statDictionary.ContainsKey(indexer) == false)
                {
                    Debug.LogWarning("Stat of Given Type is Doesn't Existed");
                    return float.NaN;
                }

                return statDictionary[indexer];
            }
        }

        protected override void OnTableInitialized()
        {
            base.OnTableInitialized();

            statDictionary.Add(EFarmerStatType.MoveSpeed, TableRow.moveSpeedIncreaseValue);
            statDictionary.Add(EFarmerStatType.Health, TableRow.healthIncreaseValue);
            statDictionary.Add(EFarmerStatType.FarmingSkill, TableRow.farmingSkillIncreaseValue);
            statDictionary.Add(EFarmerStatType.AdventureSkill, TableRow.adventureSkillIncreaseValue);
        }
    }
}



