using System;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct CalculateRepeatQuestTargetValue
    {
        public int targetValue;

        public CalculateRepeatQuestTargetValue(RepeatQuestTableRow tableRow, int repeatCount)
        {
            float multiplier = 1 + tableRow.targetValueMultiplierByRepeatCount * repeatCount;
            targetValue = (int)Math.Floor(tableRow.targetValue * multiplier);
        }
    }
}