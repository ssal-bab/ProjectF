using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class AdventureConfigTableRow : DataTableRow
    {
        public string key;
        public float value;
    }

    public partial class AdventureConfigTable : DataTable<AdventureConfigTableRow> { }
}
