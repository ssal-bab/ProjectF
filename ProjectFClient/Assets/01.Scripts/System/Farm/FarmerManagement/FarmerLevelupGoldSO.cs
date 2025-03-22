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
    [CreateAssetMenu(menuName = "SO/Farm/FarmerLevelupGold")]
    public class FarmerLevelupGoldSO : DataTableSO<FarmerLevelupGoldTable, FarmerLevelupGoldTableRow>
    {
        private Dictionary<ERarity, float> baseGoldDictionary;
        public Dictionary<ERarity, float> BaseGoldDictionary => baseGoldDictionary;
        private Dictionary<ERarity, float> multiplierGoldDictionary;
        public Dictionary<ERarity, float> MultiplierGoldDictionary => multiplierGoldDictionary;

        protected override void OnTableInitialized()
        {
            base.OnTableInitialized();
            baseGoldDictionary = new ();
            multiplierGoldDictionary = new ();

            #region Generate Base Gold Dictionary

            AddBaseGoldValue(ERarity.Common, TableRow.CommonBaseValue);
            AddBaseGoldValue(ERarity.Uncommon, TableRow.UnCommonBaseValue);
            AddBaseGoldValue(ERarity.Rare, TableRow.RareBaseValue);
            AddBaseGoldValue(ERarity.Epic, TableRow.EpicBaseValue);
            AddBaseGoldValue(ERarity.Legendary, TableRow.LegendaryBaseValue);
            AddBaseGoldValue(ERarity.Mythic, TableRow.MythicBaseValue);

            #endregion

            #region Generate Multiplier Gold Dictionary

            AddMultiplierGoldValue(ERarity.Common, TableRow.CommonMultiplierValue);
            AddMultiplierGoldValue(ERarity.Uncommon, TableRow.UnCommonMultiplierValue);
            AddMultiplierGoldValue(ERarity.Rare, TableRow.RareMultiplierValue);
            AddMultiplierGoldValue(ERarity.Epic, TableRow.EpicMultiplierValue); 
            AddMultiplierGoldValue(ERarity.Legendary, TableRow.LegendaryMultiplierValue);
            AddMultiplierGoldValue(ERarity.Mythic, TableRow.MythicMultiplierValue);
            #endregion
        }

        public void AddBaseGoldValue(ERarity rarity, int value)
        {
            if (baseGoldDictionary.ContainsKey(rarity))
            {
                Debug.LogWarning($"Warning: {rarity} has already registered");
                return;
            }

            baseGoldDictionary.Add(rarity, value);
        }

        public void AddMultiplierGoldValue(ERarity rarity, float value)
        {
            if (multiplierGoldDictionary.ContainsKey(rarity))
            {
                Debug.LogWarning($"Warning: {rarity} has already registered");
                return;
            }

            multiplierGoldDictionary.Add(rarity, value);
        }
    }
}
