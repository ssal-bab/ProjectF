using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public abstract partial class QuestTableRow : DataTableRow
    {
        public EActionType actionType;
        public int targetValue;
        public string rewardData;
    }

    public abstract partial class QuestTable<TRow> : DataTable<TRow> where TRow : QuestTableRow { }
}