using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureTableRow : DataTableRow
    {
        public string nameLocalKey;
    }

    public partial class AdventureTable : DataTable<AdventureTableRow> { }
}
