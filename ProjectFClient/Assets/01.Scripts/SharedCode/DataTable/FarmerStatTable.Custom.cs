using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
	public partial class FarmerStatTableRow : DataTableRow
	{
    }

	public partial class FarmerStatTable : DataTable<FarmerStatTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }

        public Dictionary<EFarmerStatType, float> GetFarmerStat(int farmerID, int level)
        {
            int multiplier = level - 1;
            var row = this[farmerID];
            
            var dic = new Dictionary<EFarmerStatType, float>();

            dic.Add(EFarmerStatType.MoveSpeed, row.moveSpeedBaseValue + (row.moveSpeedIncreaseValue * multiplier));
            dic.Add(EFarmerStatType.Health, row.healthBaseValue + (row.healthIncreaseValue * multiplier));
            dic.Add(EFarmerStatType.FarmingSkill, row.farmingSkillBaseValue + (row.farmingSkillIncreaseValue * multiplier));
            dic.Add(EFarmerStatType.AdventureSkill, row.adventureSkillBaseValue + (row.adventureSkillIncreaseValue * multiplier));

            return dic;
        }
    }
}