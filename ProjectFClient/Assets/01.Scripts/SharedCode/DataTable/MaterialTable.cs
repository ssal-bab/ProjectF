using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class MaterialTableRow : DataTableRow
    {
    }

    public partial class MaterialTable : DataTable<MaterialTableRow> { }
}