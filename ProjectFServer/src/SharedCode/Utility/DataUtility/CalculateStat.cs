using ProjectF.DataTables;

namespace ProjectF
{
    public struct CalculateStat
    {
        public float currentStat;

        public CalculateStat(StatTableData statData, int currentLevel)
        {
            int step = (currentLevel - statData.levelStart) / statData.levelStep;
            currentStat = statData.baseValue + statData.increaseValue * step;
        }
    }
}