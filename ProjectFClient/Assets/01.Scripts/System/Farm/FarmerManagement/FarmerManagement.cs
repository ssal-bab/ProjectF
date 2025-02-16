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
        
        /// <summary>
        /// �ϲ��� ID �Է� �� �ش� �ϲ��� ���� ���� ������ �ε�
        /// </summary>
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

        /// <summary>
        /// �ϲ� ���� ����, farmer = ������ �ϲ�, currentLevel = ���� ��� ����, targetLevel = ��ǥ ��� ����
        /// ���� �ϲ��� currentLevel�� �޾ƿ� �� ���� �Ű������� ����
        /// </summary>
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

        /// <summary>
        /// �ϲ� ���� ����, farmer = ������ �ϲ�, currentLevel = ���� ���� ����, targetLevel = ��ǥ ���� ����
        /// ���� �ϲ��� currentLevel�� �޾ƿ� �� ���� �Ű������� ����
        /// </summary>
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

        /// <summary>
        /// farmer = �� �ϲ�, farmingLevel = farmer�� ��� ����, adventureLevel = farmer�� ���� ����
        /// </summary>
        public int FarmerSell(Farmer farmer, /*EFarmerGrade grade*/ int farmingLevel, int adventureLevel)
        {
            // float farmingMultiplier = farmingLevel * DataDefine.Farming_Level_Sales_Multiplier;
            // float adventureMultiplier = adventureLevel * DataDefine.Adventure_Level_Sales_Multiplier;
            /*float gradeMultiplier = grade * DataDefine.Faemer_Grade_Sales_Multiplier;*/

            // return Mathf.FloorToInt(farmingMultiplier + adventureMultiplier /*+ gradeMultiplier*/);
            return 0;
        }
    }
}
