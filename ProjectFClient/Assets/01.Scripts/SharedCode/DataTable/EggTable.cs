using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public class EggTableRow : DataTableRow
    {
        public ERariry rarity;
        public float hatchingTime;
    }

    public class EggTable : DataTable<EggTableRow> { }
}