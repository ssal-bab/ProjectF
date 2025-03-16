using H00N.Resources;
using UnityEngine;
using System.Collections.Generic;
using H00N.Stats;
using System;
using ProjectF.Datas;
using System.Threading.Tasks;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        private Dictionary<int, FarmerIncreaseStatSO> increaseStatDataDictionary = new();
        private FarmerSalesMultiplierSO salesMultiplierData;
        
        public async void InitializeFarmerIncreaseStatAsync()
        {
            foreach(var farmerData in GameInstance.MainUser.farmerData.farmerList.Values)
            {
                int farmerID = farmerData.farmerID;

                if(!increaseStatDataDictionary.ContainsKey(farmerID))
                {
                    var incData = await ResourceManager.LoadResourceAsync<FarmerIncreaseStatSO>($"FarmerIncreaseStat_{farmerID}");
                    increaseStatDataDictionary.Add(farmerID, incData);
                    continue;
                }

                Debug.LogWarning($"Warning:{farmerID} has already registered");
            }
        }

        public void InitializeFarmerSalesMultiplier()
        {

        }

        public void ChangeFarmerLevel(Farmer farmer, int farmerID, int targetLevel, int currentLevel)
        {
            CalculateLevelDifference(targetLevel, currentLevel, (delta, theta) =>
            {
                FarmerStatAdjustmentModifier(farmer.Stat, farmerID, delta, theta, 
                                             EFarmerStatType.MoveSpeed, 
                                             EFarmerStatType.Health, 
                                             EFarmerStatType.FarmingSkill,
                                             EFarmerStatType.AdventureSkill);
            });
        }

        private void CalculateLevelDifference(int targetLevel, int currentLevel, Action<int, int> applyStats)
        {
            int delta = targetLevel - currentLevel;
            if (delta == 0) return;

            int theta = Mathf.Abs(delta);
            applyStats?.Invoke(delta, theta);
        }

        private void FarmerStatAdjustmentModifier(FarmerStat stat, int farmerID, int delta, int theta, params EFarmerStatType[] statTypes)
        {
            var incData = increaseStatDataDictionary[farmerID];

            Action<EFarmerStatType> modifyStat = delta > 0 ? 
            (t => stat.AddModifier(t, EStatModifierType.Addend, incData[t] * theta)) : 
            (t => stat.RemoveModifier(t, EStatModifierType.Addend, incData[t] * theta));

            foreach (EFarmerStatType t in statTypes)
            {
                modifyStat(t);
            }
        }

        public async Task<int> FarmerSellAsync(string farmerUUID, int farmingLevel, int adventureLevel)
        {
            int farmerID = GameInstance.MainUser.farmerData.farmerList[farmerUUID].farmerID;
            FarmerSO farmerSO = await ResourceManager.LoadResourceAsync<FarmerSO>($"Farmer_{farmerID}");
            ERarity rarity = farmerSO.TableRow.rarity;

            float farmingMultiplier = farmingLevel * salesMultiplierData.LevelSalesMultiplierValue;
            float gradeMultiplier = (int)rarity * salesMultiplierData.GradeSalesMultiplierValue;

            GameInstance.MainUser.farmerData.farmerList.Remove(farmerUUID);

            return Mathf.FloorToInt(farmingMultiplier + gradeMultiplier);
        }
    }
}
