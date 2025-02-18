using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class FarmerTableRow : DataTableRow
    {
        public string nameLocalKey;
    }

    public partial class FarmerTable : DataTable<FarmerTableRow> { }
}