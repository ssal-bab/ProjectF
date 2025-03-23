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
        public string questName;
        public string parameters;
        public string rewordType1;
        public int rewordAmount1;
        public string rewordType2;
        public int rewordAmount2;
        public string rewordType3;
        public int rewordAmount3;
    }

    public partial class QuestTable : DataTable<QuestTableRow> { }
}