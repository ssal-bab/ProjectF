using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public abstract partial class ConfigTableRow<TValueType> : DataTableRow
    {
        public string key;
        public TValueType value;
    }

    public abstract partial class ConfigTable<TRow, TValueType> : DataTable<TRow> where TRow : ConfigTableRow<TValueType> { }
}