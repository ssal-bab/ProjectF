using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class NestTableRow : FacilityTableRowBase
    {
        public int level;
        public int eggStoreLimit;
        public int farmerStoreLimit;
    }

    public class NestTable : DataTable<NestTableRow> 
    { 
        private Dictionary<int, NestTableRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, NestTableRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public NestTableRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out NestTableRow tableRow);
            return tableRow;
        }
    }
}