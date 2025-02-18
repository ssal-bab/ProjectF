using System;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class EggTableRow : DataTableRow
    {
        public ERariry rarity;
        public float hatchingTime;
    }

    public partial class EggTable : DataTable<EggTableRow> { }
}