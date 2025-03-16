using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class EggTableRow : DataTableRow
    {
        public ERarity rarity;
        public float hatchingTime;
    }

    public partial class EggTable : DataTable<EggTableRow> { }
}
