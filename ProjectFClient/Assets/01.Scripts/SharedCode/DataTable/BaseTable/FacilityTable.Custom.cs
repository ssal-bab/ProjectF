using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public abstract partial class FacilityTableRow : DataTableRow
    {
    }

    public abstract partial class FacilityTable<TRow> : DataTable<TRow> where TRow : FacilityTableRow
    {
        private Dictionary<int, TRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, TRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public TRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out TRow tableRow);
            return tableRow;
        }
    }
}