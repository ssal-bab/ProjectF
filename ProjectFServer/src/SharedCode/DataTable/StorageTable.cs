using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class StorageTableRow : FacilityTableRowBase
    {
        public int level;
        public int storeLimit;
        public float priceMultiplier;
    }

    public partial class StorageTable : DataTable<StorageTableRow> { }
}