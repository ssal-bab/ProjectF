using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class ConfigTableRow<TValueType> : DataTableRow
    {
    }

    public abstract partial class ConfigTable<TRow, TValueType> : DataTable<TRow> where TRow : ConfigTableRow<TValueType>
    {
        private Dictionary<string, TValueType> keyValueTable = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            keyValueTable = new Dictionary<string, TValueType>();
            foreach(var tableRow in table.Values)
            {
                if(keyValueTable.ContainsKey(tableRow.key))
                    continue;

                keyValueTable.Add(tableRow.key, tableRow.value);
            }
        }

        public TValueType GetValue(string key)
        {
            keyValueTable.TryGetValue(key, out TValueType value);
            return value;
        }
    }
}