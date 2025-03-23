using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class QuestTableRow : DataTableRow
    {
        public EQuestType questType;
        public string parameters;
    }

    public partial class QuestTable : DataTable<QuestTableRow> { }
}