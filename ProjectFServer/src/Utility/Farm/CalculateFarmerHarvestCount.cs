using System;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct CalculateFarmerHarvestCount
    {
        public int harvestCount;

        public CalculateFarmerHarvestCount(FarmerStatTableRow statTableRow, int level)
        {
            float farmingSkill = new CalculateStat(statTableRow.farmingSkill, level).currentStat;
            int harvestMaxCount = (int)(farmingSkill / DataTableManager.GetTable<FarmerConfigTable>().FarmingSkillFactor) + 1;
            harvestCount = new Random().Next(1, harvestMaxCount + 1);
        }
    }
}