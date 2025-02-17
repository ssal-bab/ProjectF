using H00N.Resources;
using UnityEngine;
using System.Collections.Generic;
using H00N.Stats;
using System;
using ProjectF.Datas;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        private Dictionary<int, FarmerIncreaseStatSO> increaseStatDataDictionary = new();
        
        public async void InitializeFarmerIncreaseStatAsync(int id)
        {
            if(!increaseStatDataDictionary.ContainsKey(id))
            {
                var incData = await ResourceManager.LoadResourceAsync<FarmerIncreaseStatSO>($"FarmerIncreaseStat_{id}");
                increaseStatDataDictionary.Add(id, incData);
                return;
            }

            Debug.LogWarning($"Warning:{id} has already registered");
        }
        public void ChangeFarmingLevel(Farmer farmer, int id, int targetLevel, int currentLevel)
        {
            CalculateLevelDifference(farmer, id, targetLevel, currentLevel, (delta, theta) =>
            {
                FarmerStatAdjustmentModifier(farmer.Stat, id, delta, theta, 
                                             EFarmerStatType.MoveSpeed, 
                                             EFarmerStatType.Health, 
                                             EFarmerStatType.FarmingSkill);
            });
        }

        public void ChangeAdventureLevel(Farmer farmer, int id, int targetLevel, int currentLevel)
        {
            CalculateLevelDifference(farmer, id, targetLevel, currentLevel, (delta, theta) =>
            {
                FarmerStatAdjustmentModifier(farmer.Stat, id, delta, theta, EFarmerStatType.AdventureSkill);
            });
        }

        private void CalculateLevelDifference(Farmer farmer, int id, int targetLevel, int currentLevel, Action<int, int> applyStats)
        {
            int delta = targetLevel - currentLevel;
            if (delta == 0) return;

            int theta = Mathf.Abs(delta);
            applyStats?.Invoke(delta, theta);
        }

        private void FarmerStatAdjustmentModifier(FarmerStat stat, int id, int delta, int theta, params EFarmerStatType[] statTypes)
        {
            var incData = increaseStatDataDictionary[id];

            Action<EFarmerStatType> modifyStat = delta > 0 ? 
            (t => stat.AddModifier(t, EStatModifierType.Addend, incData[t] * theta)) : 
            (t => stat.RemoveModifier(t, EStatModifierType.Addend, incData[t] * theta));

            foreach (EFarmerStatType t in statTypes)
            {
                modifyStat(t);
            }
        }

        public int FarmerSell(Farmer farmer, ERariry rarity, int farmingLevel, int adventureLevel)
        {
            float farmingMultiplier = farmingLevel * DataDefine.FARMING_LEVEL_SALES_MULTIPLIER;
            float adventureMultiplier = adventureLevel * DataDefine.ADVENTURE_LEVEL_SALES_MULTIPLIER;
            float gradeMultiplier = (int)rarity * DataDefine.FARMER_GRADE_SALES_MULTIPLIER;

            return Mathf.FloorToInt(farmingMultiplier + adventureMultiplier + gradeMultiplier);
        }
    }
}
