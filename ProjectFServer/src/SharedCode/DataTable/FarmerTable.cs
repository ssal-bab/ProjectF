using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class FarmerTableRow : DataTableRow
    {
        public string nameLocalKey;
    }

    public class FarmerTable : DataTable<FarmerTableRow> { }
}