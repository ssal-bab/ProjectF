using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class LevelTableRow : DataTableRow
    {
        public int level;
        public int gold;
        public int gem;
    }

    public abstract partial class LevelTable<TRow> : DataTable<TRow> where TRow : LevelTableRow { }
}