using System.Collections;
using System.Collections.Generic;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct GetFarmerStat 
    {
        public Dictionary<EFarmerStatType, float> statDictionary;

        public GetFarmerStat(FarmerStatTableRow tableRow, int level)
        {
            statDictionary = new Dictionary<EFarmerStatType, float>()
            {
                [EFarmerStatType.MoveSpeed] = new CalculateStat(tableRow.moveSpeed, level).currentStat,
                [EFarmerStatType.Health] = new CalculateStat(tableRow.health, level).currentStat,
                [EFarmerStatType.FarmingSkill] = new CalculateStat(tableRow.farmingSkill, level).currentStat,
                [EFarmerStatType.AdventureSkill] = new CalculateStat(tableRow.adventureSkill, level).currentStat,
                [EFarmerStatType.AdventureHealth] = new CalculateStat(tableRow.health, level).currentStat
            };
        }
    }
}
