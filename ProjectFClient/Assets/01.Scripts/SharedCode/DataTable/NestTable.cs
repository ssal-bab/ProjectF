using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class NestTableRow : FacilityTableRow
    {
        public int eggStoreLimit;
        public int farmerStoreLimit;
    }

    public partial class NestTable : FacilityTable<NestTableRow> { }
}
