using System;
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

    public class CropStorageTable : DataTable<CropStorageTableRow> { }
}