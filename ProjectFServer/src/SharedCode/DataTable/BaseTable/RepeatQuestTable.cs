using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class RepeatQuestTableRow : QuestTableRow
    {
        public float targetValueMultiplierByRepeatCount;
        public float rewardMultiplierByRepeatCount;
    }

    public abstract partial class RepeatQuestTable<TRow> : DataTable<TRow> where TRow : RepeatQuestTableRow { }
}