using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public class StatTableData
    {
        public float baseValue;
        public float increaseValue;
        public int levelStart;
        public int levelStep;

        public StatTableData(float[] stat)
        {
            baseValue = stat[0];
            increaseValue = stat[1];
            levelStart = (int)stat[2];
            levelStep = (int)stat[3];
        }
    }

	public partial class FarmerStatTableRow : DataTableRow
	{
        public StatTableData moveSpeed;
        public StatTableData health;
        public StatTableData farmingSkill;
        public StatTableData adventureSkill;
    }

	public partial class FarmerStatTable : DataTable<FarmerStatTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            foreach(FarmerStatTableRow tableRow in this)
            {
                tableRow.moveSpeed = new StatTableData(tableRow.moveSpeedStat);
                tableRow.health = new StatTableData(tableRow.healthStat);
                tableRow.farmingSkill = new StatTableData(tableRow.farmingSkillStat);
                tableRow.adventureSkill = new StatTableData(tableRow.adventureSkillStat);
            }
        }

        public Dictionary<EFarmerStatType, float> GetFarmerStat(int farmerID, int level)
        {
            int multiplier = level - 1;
            var row = this[farmerID];
            
            var dic = new Dictionary<EFarmerStatType, float>();

            dic.Add(EFarmerStatType.MoveSpeed, row.moveSpeed.baseValue + (row.moveSpeed.increaseValue * multiplier));
            dic.Add(EFarmerStatType.Health, row.health.baseValue + (row.health.increaseValue * multiplier));
            dic.Add(EFarmerStatType.FarmingSkill, row.farmingSkill.baseValue + (row.farmingSkill.increaseValue * multiplier));
            dic.Add(EFarmerStatType.AdventureSkill, row.adventureSkill.baseValue + (row.adventureSkill.increaseValue * multiplier));

            return dic;
        }
    }
}