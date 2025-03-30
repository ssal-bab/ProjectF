using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class RepeatQuestTableRow : QuestTableRow
    {
        public float targetValueMultiplierByRepeatCount; // targetVlaue * (1 + 반복 횟수 * targetValueMultiplierByRepeatCount)
        public float rewardMultiplierByRepeatCount; // rewardItemAmount * (1 + 반복 횟수 * rewardMultiplierByRepeatCount)
    }

    public abstract partial class RepeatQuestTable<TRow> : QuestTable<TRow> where TRow : RepeatQuestTableRow { }
}