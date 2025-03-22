using System.Collections;
using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF.DataTables
{
    public partial class FarmerLevelupGoldTableRow : DataTableRow
    {
        
    }

    public partial class FarmerLevelupGoldTable : DataTable<FarmerLevelupGoldTableRow> 
    { 
        private Dictionary<ERarity, float> baseGoldDictionary = new();
        public Dictionary<ERarity, float> BaseGoldDictionary => baseGoldDictionary;
        private Dictionary<ERarity, float> multiplierGoldDictionary = new();
        public Dictionary<ERarity, float> MultiplierGoldDictionary => multiplierGoldDictionary;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            baseGoldDictionary = new ();
            multiplierGoldDictionary = new ();

            var tableRow = this[0];

            #region Generate Base Gold Dictionary

            AddBaseGoldValue(ERarity.Common, tableRow.CommonBaseValue);
            AddBaseGoldValue(ERarity.Uncommon, tableRow.UnCommonBaseValue);
            AddBaseGoldValue(ERarity.Rare, tableRow.RareBaseValue);
            AddBaseGoldValue(ERarity.Epic, tableRow.EpicBaseValue);
            AddBaseGoldValue(ERarity.Legendary, tableRow.LegendaryBaseValue);
            AddBaseGoldValue(ERarity.Mythic, tableRow.MythicBaseValue);

            #endregion

            #region Generate Multiplier Gold Dictionary

            AddMultiplierGoldValue(ERarity.Common, tableRow.CommonMultiplierValue);
            AddMultiplierGoldValue(ERarity.Uncommon, tableRow.UnCommonMultiplierValue);
            AddMultiplierGoldValue(ERarity.Rare, tableRow.RareMultiplierValue);
            AddMultiplierGoldValue(ERarity.Epic, tableRow.EpicMultiplierValue); 
            AddMultiplierGoldValue(ERarity.Legendary, tableRow.LegendaryMultiplierValue);
            AddMultiplierGoldValue(ERarity.Mythic, tableRow.MythicMultiplierValue);
            #endregion
        }

        public void AddBaseGoldValue(ERarity rarity, int value)
        {
            if (baseGoldDictionary.ContainsKey(rarity))
            {
                return;
            }

            baseGoldDictionary.Add(rarity, value);
        }

        public void AddMultiplierGoldValue(ERarity rarity, float value)
        {
            if (multiplierGoldDictionary.ContainsKey(rarity))
            {
                return;
            }

            multiplierGoldDictionary.Add(rarity, value);
        }
    }
}