using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class MaterialTableRow : DataTableRow
    {
    }

    public partial class MaterialTable : DataTable<MaterialTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}