using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class CropStorageTableRow : DataTableRow
    {
        public int level;
        public int storeLimit;
        public float priceMultiplier;
        public int upgradeGold;
        public int materialItemID;
        public int materialItemCount;        					
    }

    public class CropStorageTable : DataTable<CropStorageTableRow> { }
}