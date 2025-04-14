using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class UpgradeCostTableRow : DataTableRow
    {
        public int level;
        public int costItemID;
        public int costValue;
    }

    public abstract partial class UpgradeCostTable<TRow> : DataTable<TRow> where TRow : UpgradeCostTableRow { }
}