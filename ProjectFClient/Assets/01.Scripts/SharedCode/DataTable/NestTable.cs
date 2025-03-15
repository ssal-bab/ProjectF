using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class NestTableRow : FacilityTableRow
    {
        public int eggStoreLimit;
        public int farmerStoreLimit;
    }

    public partial class NestTable : FacilityTable<NestTableRow> { }
}