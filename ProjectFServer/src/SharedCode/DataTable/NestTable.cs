using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class NestTableRow : FacilityTableRowBase
    {
        public int level;
        public int eggStoreLimit;
        public int farmerStoreLimit;
    }

    public partial class NestTable : DataTable<NestTableRow> { }
}