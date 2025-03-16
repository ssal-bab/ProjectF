using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class StorageTableRow : FacilityTableRow
    {
        public int storeLimit;
        public float priceMultiplier;
    }

    public partial class StorageTable : FacilityTable { }
}
