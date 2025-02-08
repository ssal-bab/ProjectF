using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class CropStorageTableRow : FacilityTableRowBase
    {
        public int level;
        public int storeLimit;
        public float priceMultiplier;
    }

    public class CropStorageTable : DataTable<CropStorageTableRow> 
    { 
        private Dictionary<int, CropStorageTableRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, CropStorageTableRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public CropStorageTableRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out CropStorageTableRow tableRow);
            return tableRow;
        }
    }
}