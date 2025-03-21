using H00N.Resources;
using UnityEngine;
using System.Collections.Generic;
using H00N.Stats;
using System;
using ProjectF.Datas;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectFServer.Networks.Packets;
using UnityEditor.PackageManager.Requests;

namespace ProjectF.Farms
{
    public class FarmerManagement : MonoBehaviour
    {
        private Dictionary<int, FarmerIncreaseStatSO> increaseStatDataDictionary = new();
        
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

        public async Task<FarmerLevelupResponse> ChangeFarmerLevel(Farmer farmer, string farmerUUID, int farmerID, int targetLevel, int currentLevel)
        {
            var req = new FarmerLevelupRequest(farmerUUID, targetLevel);
            var response = await NetworkManager.Instance.SendWebRequestAsync<FarmerLevelupResponse>(req);

            if(response.result != ENetworkResult.Success) 
                return response;

            CalculateLevelDifference(targetLevel, currentLevel, (delta, theta) =>
            {
                FarmerStatAdjustmentModifier(farmer.Stat, farmerID, delta, theta, 
                                             EFarmerStatType.MoveSpeed, 
                                             EFarmerStatType.Health, 
                                             EFarmerStatType.FarmingSkill,
                                             EFarmerStatType.AdventureSkill);
            });

            return response;
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

        public async Task<FarmerSalesResponse> FarmerSaleAsync(string farmerUUID, int farmingLevel, int adventureLevel)
        {
            ERarity rarity = GameInstance.MainUser.farmerData.farmerList[farmerUUID].rarity;

            FarmerConfigTable farmerConfigTable = DataTableManager.GetTable<FarmerConfigTable>();
            float farmingMultiplier = farmingLevel * farmerConfigTable.LevelSalesMultiplierValue;
            float gradeMultiplier = (int)rarity * farmerConfigTable.GradeSalesMultiplierValue;

            int salesAllowance = Mathf.FloorToInt(farmingMultiplier + gradeMultiplier);
            
            var req = new FarmerSalesRequest(farmerUUID, salesAllowance);
            return await NetworkManager.Instance.SendWebRequestAsync<FarmerSalesResponse>(req);
        }
    }
}
