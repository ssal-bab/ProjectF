using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using H00N.DataTables;
using Microsoft.VisualBasic;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF.DataTables
{
    public partial class FarmerLevelupGoldTableRow : DataTableRow
    {
        
    }

    public struct LevelupGoldGroup
    {
        public int baseValue;
        public float multiplierValue;

        public LevelupGoldGroup(int baseValue, float multiplierValue)
        {
            this.baseValue = baseValue;
            this.multiplierValue = multiplierValue;
        }
    }

    public partial class FarmerLevelupGoldTable : DataTable<FarmerLevelupGoldTableRow> 
    { 
        private Dictionary<ERarity, LevelupGoldGroup> levelupGoldGroupDictionary;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            levelupGoldGroupDictionary = new Dictionary<ERarity, LevelupGoldGroup>();

            foreach(var row in this)
            {
                levelupGoldGroupDictionary.Add(row.rarity, new LevelupGoldGroup(row.baseValue, row.multiplierValue));
            }
        }

        public LevelupGoldGroup GetLevelupGoldGroup(ERarity rarity)
        {
            if(levelupGoldGroupDictionary.TryGetValue(rarity, out var goldGroup))
            {
                return goldGroup;
            }

            return new LevelupGoldGroup();
        }
    }
}