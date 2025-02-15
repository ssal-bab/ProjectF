using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class StorageTableRow : FacilityTableRowBase
    {
        public int level;
        public int storeLimit;
        public float priceMultiplier;
    }

    public class StorageTable : DataTable<StorageTableRow> 
    { 
        private Dictionary<int, StorageTableRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, StorageTableRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public StorageTableRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out StorageTableRow tableRow);
            return tableRow;
        }
    }
}