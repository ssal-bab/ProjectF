using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class StorageTableRow : FacilityTableRow
    {
        public int storeLimit;
        public float priceMultiplier;
    }

    public partial class StorageTable : FacilityTable<StorageTableRow> { }
}