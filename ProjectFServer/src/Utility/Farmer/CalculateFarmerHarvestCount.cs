using System;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct CalculateFarmerHarvestCount
    {
        public int harvestCount;

        public CalculateFarmerHarvestCount(FarmerLevelTableRow levelTableRow, int level)
        {
            int harvestMaxCount = (int)(levelTableRow.farmingSkill / DataTableManager.GetTable<FarmerConfigTable>().FarmingSkillFactor) + 1;
            harvestCount = new Random().Next(1, harvestMaxCount + 1);
        }
    }
}